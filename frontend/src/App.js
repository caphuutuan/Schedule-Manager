import React, { useState, useEffect, useMemo } from 'react';
import * as api from './services/api';
import Management from './components/Management';
import './components/SchoolSelection.css';

const App = () => {
  const [view, setView] = useState('schedules'); // schedules | management
  const [selectedType, setSelectedType] = useState('class'); // class | teacher | department
  const [selectedId, setSelectedId] = useState('');
  const [schedules, setSchedules] = useState([]);
  const [entities, setEntities] = useState([]);
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(false);
  const [error, setError] = useState(null);
  const [selectedSchool, setSelectedSchool] = useState(null);
  const [schools, setSchools] = useState([]);
  const [initLoading, setInitLoading] = useState(true);

  // Load selected school from localStorage on mount
  useEffect(() => {
    const saved = localStorage.getItem('selectedSchool');
    if (saved) {
      setSelectedSchool(JSON.parse(saved));
    }
    const fetchSchoolsList = async () => {
      try {
        const data = await api.getSchools();
        setSchools(data);
      } catch (err) {
        console.error('Failed to fetch schools:', err);
      } finally {
        setInitLoading(false);
      }
    };
    fetchSchoolsList();
  }, []);

  const handleSelectSchool = (school) => {
    setSelectedSchool(school);
    localStorage.setItem('selectedSchool', JSON.stringify(school));
  };

  const handleSwitchSchool = () => {
    setSelectedSchool(null);
    localStorage.removeItem('selectedSchool');
  };

  const schoolId = selectedSchool?.id;

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
    if (view !== 'schedules' || !schoolId) return;
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
      else setSelectedId('');
    } catch (err) {
      setError(`Failed to load ${selectedType}es: ` + err.message);
    } finally {
      setLoading(false);
    }
  }, [selectedType, schoolId, view]);

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
      const data = await api.getSchedules(schoolId, params);
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

  const renderSchoolSelection = () => (
    <div className="school-selection-container">
      <div className="header-actions">
        <h1>Select Your School</h1>
        <p style={{ textAlign: 'center', color: 'var(--text-muted)', marginBottom: '20px' }}>
          Please choose a school to manage schedules and data.
        </p>
      </div>
      
      {initLoading ? (
        <div className="no-data"><span className="loader"></span> Loading schools...</div>
      ) : (
        <div className="school-grid">
          {schools.map(school => (
            <div 
              key={school.id} 
              className="school-card" 
              onClick={() => handleSelectSchool(school)}
            >
              <span className={`level-badge level-${school.level}`}>
                {school.level === 1 ? 'Elementary' : 
                 school.level === 2 ? 'Middle' : 
                 school.level === 3 ? 'High' : 'K-12'}
              </span>
              <h3>{school.name}</h3>
              <p style={{ fontSize: '0.875rem', color: 'var(--text-muted)' }}>
                Click to enter this school dashboard.
              </p>
            </div>
          ))}
          {schools.length === 0 && (
            <div className="no-data">No schools found in the system.</div>
          )}
        </div>
      )}
    </div>
  );

  const renderSchedulesView = () => (
    <div className="container">
      <div className="header-actions">
        <h1>Schedule Manager</h1>
        <div style={{ textAlign: 'center', color: 'var(--text-muted)', marginTop: '-20px', marginBottom: '20px' }}>
           {selectedSchool?.name}
        </div>
      </div>

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

  return (
    <div className="app-layout">
      {!selectedSchool ? renderSchoolSelection() : (
        <>
          <nav className="main-nav">
            <div className="nav-logo">EduSchedule</div>
            
            <div className="nav-school-info">
              <span>{selectedSchool.name}</span>
              <button className="switch-school-btn" onClick={handleSwitchSchool}>
                Switch School
              </button>
            </div>

            <div className="nav-links">
              <button
                className={view === 'schedules' ? 'active' : ''}
                onClick={() => setView('schedules')}
              >
                Schedules
              </button>
              <button
                className={view === 'management' ? 'active' : ''}
                onClick={() => setView('management')}
              >
                Management
              </button>
            </div>
          </nav>

          {view === 'schedules' ? renderSchedulesView() : <Management school={selectedSchool} />}
        </>
      )}
    </div>
  );
};

export default App;
