import React, { useState, useEffect, useMemo } from 'react';
import * as api from './services/api';

const App = () => {
  const [selectedType, setSelectedType] = useState('class'); // class | teacher | department
  const [selectedId, setSelectedId] = useState('');
  const [schedules, setSchedules] = useState([]);
  const [entities, setEntities] = useState([]);
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(false);
  const [error, setError] = useState(null);

  const schoolId = 1; // Default school ID as per requirements

  // Generate 52 weeks for 2026 (starting from 1st Jan 2026)
  const weeks = useMemo(() => {
    const weekItems = [];
    let current = new Date('2026-01-05'); // First Monday of 2026
    for (let i = 1; i <= 52; i++) {
      const start = new Date(current);
      const end = new Date(current);
      end.setDate(end.getDate() + 6);
      
      const label = `Tuần ${i}: ${start.toLocaleDateString('vi-VN')} - ${end.toLocaleDateString('vi-VN')}`;
      weekItems.push({
        id: i,
        label,
        fromDate: start.toISOString().split('T')[0],
        toDate: end.toISOString().split('T')[0]
      });
      current.setDate(current.getDate() + 7);
    }
    return weekItems;
  }, []);

  const [selectedWeekIndex, setSelectedWeekIndex] = useState(0);

  // Load entities (classes/teachers/departments) when tab changes
  const loadEntities = React.useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      let data = [];
      if (selectedType === 'class') {
        data = await api.getClasses(schoolId);
      } else if (selectedType === 'teacher') {
        data = await api.getTeachers(schoolId);
      } else if (selectedType === 'department') {
        data = await api.getDepartments(schoolId);
      }
      setEntities(data);
      if (data.length > 0) setSelectedId(data[0].id.toString());
    } catch (err) {
      setError(`Failed to load ${selectedType}es: ` + err.message);
    } finally {
      setLoading(false);
    }
  }, [selectedType, schoolId]);

  useEffect(() => {
    loadEntities();
    setSelectedId(''); // Reset selection
    setSchedules([]); // Clear results
    setError(null);
  }, [loadEntities]);

  const handleFetchSchedule = async () => {
    if (!selectedId) {
      setError('Please select an item');
      return;
    }
    setFetching(true);
    setError(null);
    try {
      const week = weeks[selectedWeekIndex];
      const params = {
        schoolId,
        type: selectedType,
        id: selectedId,
        fromDate: week.fromDate,
        toDate: week.toDate,
      };
      const data = await api.getSchedules(params);
      // Group by DayOfWeek and sort by Period
      const sortedData = [...data].sort((a, b) => (a.dayOfWeek - b.dayOfWeek) || (a.period - b.period));
      setSchedules(sortedData);
    } catch (err) {
      setError('Failed to fetch schedule: ' + err.message);
      setSchedules([]);
    } finally {
      setFetching(false);
    }
  };

  const getDayName = (dow) => {
    const days = ['Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7', 'Chủ Nhật'];
    return days[dow - 1] || 'Unknown';
  };

  const getFormattedDate = (s) => {
    if (s.date) return new Date(s.date).toLocaleDateString('vi-VN');
    
    // Calculate date for recurring schedule based on selected week
    const week = weeks[selectedWeekIndex];
    const date = new Date(week.fromDate);
    date.setDate(date.getDate() + (s.dayOfWeek - 1));
    return date.toLocaleDateString('vi-VN');
  };

  return (
    <div className="container">
      <h1>Schedule Manager</h1>

      <div className="tabs">
        {['class', 'teacher', 'department'].map((type) => (
          <button
            key={type}
            className={`tab ${selectedType === type ? 'active' : ''}`}
            onClick={() => setSelectedType(type)}
          >
            {type.charAt(0).toUpperCase() + type.slice(1)}
          </button>
        ))}
      </div>

      <div className="form-group">
        <div>
          <label>Select {selectedType}</label>
          <select 
            value={selectedId} 
            onChange={(e) => setSelectedId(e.target.value)}
            disabled={loading}
          >
            {loading ? (
              <option>Loading...</option>
            ) : (
              entities.map((item) => (
                <option key={item.id} value={item.id}>
                  {item.name} {item.code ? `(${item.code})` : ''}
                </option>
              ))
            )}
          </select>
        </div>

        <div>
          <label>Week</label>
          <select
            value={selectedWeekIndex}
            onChange={(e) => setSelectedWeekIndex(parseInt(e.target.value))}
          >
            {weeks.map((w, index) => (
              <option key={w.id} value={index}>
                {w.label}
              </option>
            ))}
          </select>
        </div>

        <button 
          className="fetch-btn" 
          onClick={handleFetchSchedule}
          disabled={fetching || loading || !selectedId}
        >
          {fetching ? <span className="loader"></span> : 'Fetch Schedule'}
        </button>
      </div>

      {error && <div className="error-msg">{error}</div>}

      <div className="schedule-list">
        {fetching ? (
          <div className="no-data">Fetching results...</div>
        ) : schedules.length > 0 ? (
          schedules.map((s, index) => {
            const showHeader = index === 0 || s.dayOfWeek !== schedules[index - 1].dayOfWeek;
            return (
              <React.Fragment key={s.id}>
                {showHeader && (
                  <div className="day-header">
                    {getDayName(s.dayOfWeek)} — {getFormattedDate(s)}
                  </div>
                )}
                <div className="schedule-item">
                  <div className="period-badge">Period {s.period}</div>
                  <div className="subject-info">
                    <h3>{s.subjectName}</h3>
                    <p>Class: {s.className}</p>
                  </div>
                  <div className="teacher-tag">
                    {s.teacherName}
                  </div>
                </div>
              </React.Fragment>
            );
          })
        ) : !fetching && !error && (
          <div className="no-data">
            {selectedId ? 'No schedule found for this week.' : 'Select an item and click Fetch Schedule'}
          </div>
        )}
      </div>
    </div>
  );
};

export default App;
