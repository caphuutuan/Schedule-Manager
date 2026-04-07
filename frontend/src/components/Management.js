import React, { useState, useEffect, useCallback } from 'react';
import * as api from '../services/api';
import EntityForm from './EntityForm';
import ScheduleForm from './ScheduleForm';
import './Management.css';

const entityConfig = {
    classes: { label: 'Classes', api: 'getClasses', create: 'createClass', update: 'updateClass', delete: 'deleteClass' },
    teachers: { label: 'Teachers', api: 'getTeachers', create: 'createTeacher', update: 'updateTeacher', delete: 'deleteTeacher' },
    departments: { label: 'Departments', api: 'getDepartments', create: 'createDepartment', update: 'updateDepartment', delete: 'deleteDepartment' },
    subjects: { label: 'Subjects', api: 'getSubjects', create: 'createSubject', update: 'updateSubject', delete: 'deleteSubject' },
    schools: { label: 'Schools', api: 'getSchools', create: 'createSchool', update: 'updateSchool', delete: 'deleteSchool' },
    schedules: { label: 'Schedules', api: 'getAllSchedules', create: 'createSchedule', update: 'updateSchedule', delete: 'deleteSchedule' }
};

const Management = () => {
    const [activeTab, setActiveTab] = useState('classes');
    const [entities, setEntities] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showForm, setShowForm] = useState(false);
    const [editingEntity, setEditingEntity] = useState(null);
    const [schoolLevel, setSchoolLevel] = useState(3); // Default to High School (3)
    const [dropdowns, setDropdowns] = useState({
        departments: [],
        schools: []
    });

    const fetchEntities = useCallback(async () => {
        setLoading(true);
        try {
            let data;
            if (activeTab === 'schedules') {
                // For schedules tab in management, we might want a simple list or special fetch
                // Adding a placeholder getAllSchedules for now or using getSchedules with default params
                data = await api.getSchedules({ schoolId: 1 }); 
            } else if (activeTab === 'schools') {
                data = await api.getSchools();
            } else {
                data = await api[entityConfig[activeTab].api](1); // Default schoolId = 1
            }
            setEntities(data);
        } catch (error) {
            console.error('Error fetching entities:', error);
            alert('Failed to fetch entities');
        } finally {
            setLoading(false);
        }
    }, [activeTab]);

    const fetchInitialData = useCallback(async () => {
        try {
            const [deps, schoolInfo] = await Promise.all([
                api.getDepartments(1),
                api.getSchool(1)
            ]);
            setDropdowns({ departments: deps });
            if (schoolInfo) setSchoolLevel(schoolInfo.level);
        } catch (error) {
            console.error('Error fetching initial data:', error);
        }
    }, []);

    useEffect(() => {
        fetchEntities();
        fetchInitialData();
    }, [fetchEntities, fetchInitialData]);

    const handleAdd = () => {
        setEditingEntity(null);
        setShowForm(true);
    };

    const handleEdit = (entity) => {
        setEditingEntity(entity);
        setShowForm(true);
    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this item?')) {
            try {
                await api[entityConfig[activeTab].delete](id);
                fetchEntities();
            } catch (error) {
                console.error('Error deleting entity:', error);
                alert('Failed to delete entity');
            }
        }
    };

    const handleSave = async (formData) => {
        try {
            if (editingEntity) {
                await api[entityConfig[activeTab].update](editingEntity.id, formData);
            } else {
                await api[entityConfig[activeTab].create](formData);
            }
            setShowForm(false);
            fetchEntities();
        } catch (error) {
            console.error('Error saving entity:', error);
            alert('Failed to save entity');
        }
    };

    const renderTable = () => {
        if (loading) return <div className="loading">Loading...</div>;

        return (
            <div className="table-container">
                <table>
                    <thead>
                        <tr>
                            <th>ID</th>
                            {activeTab === 'schedules' ? (
                                <>
                                    <th>Subject</th>
                                    <th>Class</th>
                                    <th>Teacher</th>
                                    <th>Day/Period</th>
                                </>
                            ) : (
                                <th>Name</th>
                            )}
                            {activeTab === 'classes' && <th>Grade</th>}
                            {(activeTab === 'teachers' || activeTab === 'subjects') && <th>Department</th>}
                            <th className="actions-header">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {entities.length > 0 ? (
                            entities.map((item) => (
                                <tr key={item.id}>
                                    <td>{item.id}</td>
                                    {activeTab === 'schedules' ? (
                                        <>
                                            <td>{item.subjectName}</td>
                                            <td>{item.className}</td>
                                            <td>{item.teacherName}</td>
                                            <td>{item.dayOfWeek === 7 ? 'CN' : `T${item.dayOfWeek + 1}`} - P{item.period}</td>
                                        </>
                                    ) : (
                                        <td className="entity-name">{item.name}</td>
                                    )}
                                    {activeTab === 'classes' && <td>{item.grade}</td>}
                                    {(activeTab === 'teachers' || activeTab === 'subjects') && (
                                        <td>{item.departmentName || item.departmentId}</td>
                                    )}
                                    <td className="actions-cell">
                                        <button className="edit-btn" onClick={() => handleEdit(item)}>
                                            <i className="edit-icon">✎</i> Edit
                                        </button>
                                        <button className="delete-btn" onClick={() => handleDelete(item.id)}>
                                            <i className="delete-icon">🗑</i> Delete
                                        </button>
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan="5" className="empty-state">No {activeTab} found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        );
    };

    return (
        <div className="management-container">
            <header className="management-header">
                <h1>Data Management</h1>
                <button className="add-btn" onClick={handleAdd}>+ Add {activeTab.slice(0, -1)}</button>
            </header>

            <div className="tabs">
                {Object.entries(entityConfig).map(([key, config]) => (
                    <button
                        key={key}
                        className={`tab-btn ${activeTab === key ? 'active' : ''}`}
                        onClick={() => setActiveTab(key)}
                    >
                        {config.label}
                    </button>
                ))}
            </div>

            <main className="management-content">
                {renderTable()}
            </main>

            {showForm && (
                activeTab === 'schedules' ? (
                    <ScheduleForm
                        isOpen={showForm}
                        onClose={() => setShowForm(false)}
                        schedule={editingEntity}
                        onSave={() => {
                            setShowForm(false);
                            fetchEntities();
                        }}
                    />
                ) : (
                    <EntityForm
                        entityType={activeTab}
                        entity={editingEntity}
                        onSave={handleSave}
                        onCancel={() => setShowForm(false)}
                        dropdowns={dropdowns}
                        schoolLevel={schoolLevel}
                    />
                )
            )}
        </div>
    );
};

export default Management;
