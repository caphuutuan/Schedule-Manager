import React, { useState, useEffect } from 'react';
import * as api from '../services/api';

const ScheduleForm = ({ schedule, isOpen, onClose, onSave, schools }) => {
  const [formData, setFormData] = useState({
    schoolId: 1,
    classId: '',
    teacherId: '',
    subjectId: '',
    dayOfWeek: 1,
    period: 1,
    date: ''
  });
  const [classes, setClasses] = useState([]);
  const [teachers, setTeachers] = useState([]);
  const [subjects, setSubjects] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const loadDropdownData = React.useCallback(async () => {
    setLoading(true);
    try {
      const [classList, teacherList, subjectList] = await Promise.all([
        api.getClasses(1),
        api.getTeachers(1),
        api.getSubjects(1)
      ]);
      setClasses(classList);
      setTeachers(teacherList);
      setSubjects(subjectList);
      
      // Set defaults if creating new
      if (!schedule) {
        setFormData(prev => ({
          ...prev,
          classId: classList[0]?.id || '',
          teacherId: teacherList[0]?.id || '',
          subjectId: subjectList[0]?.id || ''
        }));
      }
    } catch (err) {
      setError('Failed to load form data: ' + err.message);
    } finally {
      setLoading(false);
    }
  }, [schedule]);

  useEffect(() => {
    if (isOpen) {
      loadDropdownData();
      if (schedule) {
        setFormData({
          ...schedule,
          date: schedule.date ? schedule.date.split('T')[0] : ''
        });
      } else {
        setFormData({
          schoolId: 1,
          classId: '',
          teacherId: '',
          subjectId: '',
          dayOfWeek: 1,
          period: 1,
          date: ''
        });
      }
    }
  }, [isOpen, schedule, loadDropdownData]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      // Clean up DTO
      const dto = {
        ...formData,
        classId: parseInt(formData.classId),
        teacherId: parseInt(formData.teacherId),
        subjectId: parseInt(formData.subjectId),
        dayOfWeek: parseInt(formData.dayOfWeek),
        period: parseInt(formData.period),
        date: formData.date || null
      };

      if (schedule?.id) {
        await api.updateSchedule(schedule.id, dto);
      } else {
        await api.createSchedule(dto);
      }
      onSave();
      onClose();
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <div className="modal-header">
          <h2>{schedule ? 'Edit Schedule' : 'Add New Schedule'}</h2>
          <button className="close-btn" onClick={onClose}>&times;</button>
        </div>

        {error && <div className="error-msg">{error}</div>}

        <form onSubmit={handleSubmit}>
          <div className="form-field">
            <label>Class</label>
            <select 
              value={formData.classId} 
              onChange={e => setFormData({...formData, classId: e.target.value})}
              required
            >
              {classes.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
          </div>

          <div className="form-field">
            <label>Teacher</label>
            <select 
              value={formData.teacherId} 
              onChange={e => setFormData({...formData, teacherId: e.target.value})}
              required
            >
              {teachers.map(t => <option key={t.id} value={t.id}>{t.name}</option>)}
            </select>
          </div>

          <div className="form-field">
            <label>Subject</label>
            <select 
              value={formData.subjectId} 
              onChange={e => setFormData({...formData, subjectId: e.target.value})}
              required
            >
              {subjects.map(s => <option key={s.id} value={s.id}>{s.name}</option>)}
            </select>
          </div>

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '15px' }}>
            <div className="form-field">
              <label>Day of Week</label>
              <select 
                value={formData.dayOfWeek} 
                onChange={e => setFormData({...formData, dayOfWeek: e.target.value})}
              >
                {[1,2,3,4,5,6,7].map(d => (
                  <option key={d} value={d}>Thứ {d === 7 ? 'Chủ Nhật' : d + 1}</option>
                ))}
              </select>
            </div>
            <div className="form-field">
              <label>Period (1-10)</label>
              <input 
                type="number" 
                min="1" 
                max="10" 
                value={formData.period} 
                onChange={e => setFormData({...formData, period: e.target.value})}
                required
              />
            </div>
          </div>

          <div className="form-field">
            <label>Specific Date (Optional)</label>
            <input 
              type="date" 
              value={formData.date} 
              onChange={e => setFormData({...formData, date: e.target.value})}
            />
            <p style={{ fontSize: '0.75rem', color: 'var(--text-muted)', marginTop: '4px' }}>
              Leave blank for a recurring weekly schedule.
            </p>
          </div>

          <div className="modal-footer">
            <button type="button" className="btn-secondary" onClick={onClose} disabled={loading}>
              Cancel
            </button>
            <button type="submit" className="fetch-btn" disabled={loading}>
              {loading ? <span className="loader"></span> : 'Save Schedule'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ScheduleForm;
