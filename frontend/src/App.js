import React, { useState, useEffect } from 'react';
import * as api from './services/api';

const App = () => {
  const [selectedType, setSelectedType] = useState('class'); // class | teacher | department
  const [selectedId, setSelectedId] = useState('');
  const [selectedDate, setSelectedDate] = useState(new Date().toISOString().split('T')[0]);
  const [schedules, setSchedules] = useState([]);
  const [entities, setEntities] = useState([]);
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(false);
  const [error, setError] = useState(null);

  const schoolId = 1; // Default school ID as per requirements

  // Load entities (classes/teachers/departments) when tab changes
  useEffect(() => {
    loadEntities();
    setSelectedId(''); // Reset selection
    setSchedules([]); // Clear results
    setError(null);
  }, [selectedType]);

  const loadEntities = async () => {
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
  };

  const handleFetchSchedule = async () => {
    if (!selectedId) {
      setError('Please select an item');
      return;
    }
    setFetching(true);
    setError(null);
    try {
      const params = {
        schoolId,
        type: selectedType,
        id: selectedId,
        date: selectedDate,
      };
      const data = await api.getSchedules(params);
      setSchedules(data);
    } catch (err) {
      setError('Failed to fetch schedule: ' + err.message);
      setSchedules([]);
    } finally {
      setFetching(false);
    }
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
          <label>Date</label>
          <input 
            type="date" 
            value={selectedDate} 
            onChange={(e) => setSelectedDate(e.target.value)}
          />
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
          schedules.map((s) => (
            <div key={s.id} className="schedule-item">
              <div className="period-badge">Period {s.period}</div>
              <div className="subject-info">
                <h3>{s.subjectName}</h3>
                <p>Class: {s.className}</p>
              </div>
              <div className="teacher-tag">
                {s.teacherName}
              </div>
            </div>
          ))
        ) : !fetching && !error && (
          <div className="no-data">
            {selectedId ? 'No schedule found for this date.' : 'Select an ID and click Fetch Schedule'}
          </div>
        )}
      </div>
    </div>
  );
};

export default App;
