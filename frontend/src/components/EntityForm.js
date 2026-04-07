import React, { useState, useEffect } from 'react';
import './EntityForm.css';

const EntityForm = ({ entityType, entity, onSave, onCancel, dropdowns, schoolLevel }) => {
    const [formData, setFormData] = useState({});
    const [errors, setErrors] = useState({});

    // Helper to get available grades based on school level
    const getGradeOptions = React.useCallback(() => {
        switch (schoolLevel) {
            case 1: // Elementary
                return [1, 2, 3, 4, 5];
            case 2: // Middle
                return [6, 7, 8, 9];
            case 3: // High
                return [10, 11, 12];
            case 4: // K12
                return [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
            default:
                return [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
        }
    }, [schoolLevel]);

    useEffect(() => {
        if (entity) {
            setFormData(entity);
        } else {
            // Initial data for new entities
            const initialData = { schoolId: 1 }; // Default schoolId
            if (entityType === 'classes') {
                initialData.grade = getGradeOptions()[0];
            }
            setFormData(initialData);
        }
    }, [entity, entityType, schoolLevel, getGradeOptions]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: name === 'grade' || name.endsWith('Id') ? parseInt(value) || 0 : value
        }));
        
        if (errors[name]) {
            setErrors(prev => {
                const newErrors = { ...prev };
                delete newErrors[name];
                return newErrors;
            });
        }
    };

    const validate = () => {
        const newErrors = {};
        if (!formData.name?.trim()) newErrors.name = 'Name is required';
        
        if (entityType === 'classes') {
            if (!formData.grade) newErrors.grade = 'Grade is required';
        }
        
        if (['teachers', 'subjects'].includes(entityType)) {
            if (!formData.departmentId) newErrors.departmentId = 'Department is required';
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (validate()) {
            onSave(formData);
        }
    };

    const renderFields = () => {
        switch (entityType) {
            case 'schools':
                return (
                    <div className="form-group">
                        <label htmlFor="name">School Name</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            value={formData.name || ''}
                            onChange={handleChange}
                            className={errors.name ? 'error' : ''}
                        />
                        {errors.name && <span className="error-text">{errors.name}</span>}
                    </div>
                );
            case 'departments':
                return (
                    <div className="form-group">
                        <label htmlFor="name">Department Name</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            value={formData.name || ''}
                            onChange={handleChange}
                            className={errors.name ? 'error' : ''}
                        />
                        {errors.name && <span className="error-text">{errors.name}</span>}
                    </div>
                );
            case 'teachers':
                return (
                    <>
                        <div className="form-group">
                            <label htmlFor="name">Teacher Name</label>
                            <input
                                type="text"
                                id="name"
                                name="name"
                                value={formData.name || ''}
                                onChange={handleChange}
                                className={errors.name ? 'error' : ''}
                            />
                            {errors.name && <span className="error-text">{errors.name}</span>}
                        </div>
                        <div className="form-group">
                            <label htmlFor="departmentId">Department</label>
                            <select
                                id="departmentId"
                                name="departmentId"
                                value={formData.departmentId || ''}
                                onChange={handleChange}
                                className={errors.departmentId ? 'error' : ''}
                            >
                                <option value="">Select Department</option>
                                {dropdowns.departments?.map(d => (
                                    <option key={d.id} value={d.id}>{d.name}</option>
                                ))}
                            </select>
                            {errors.departmentId && <span className="error-text">{errors.departmentId}</span>}
                        </div>
                    </>
                );
            case 'classes':
                return (
                    <>
                        <div className="form-group">
                            <label htmlFor="name">Class Name</label>
                            <input
                                type="text"
                                id="name"
                                name="name"
                                value={formData.name || ''}
                                onChange={handleChange}
                                className={errors.name ? 'error' : ''}
                            />
                            {errors.name && <span className="error-text">{errors.name}</span>}
                        </div>
                        <div className="form-group">
                            <label htmlFor="grade">Grade</label>
                            <select
                                id="grade"
                                name="grade"
                                value={formData.grade || ''}
                                onChange={handleChange}
                                className={errors.grade ? 'error' : ''}
                            >
                                {getGradeOptions().map(g => (
                                    <option key={g} value={g}>Grade {g}</option>
                                ))}
                            </select>
                            {errors.grade && <span className="error-text">{errors.grade}</span>}
                        </div>
                    </>
                );
            case 'subjects':
                return (
                    <>
                        <div className="form-group">
                            <label htmlFor="name">Subject Name</label>
                            <input
                                type="text"
                                id="name"
                                name="name"
                                value={formData.name || ''}
                                onChange={handleChange}
                                className={errors.name ? 'error' : ''}
                            />
                            {errors.name && <span className="error-text">{errors.name}</span>}
                        </div>
                        <div className="form-group">
                            <label htmlFor="departmentId">Department</label>
                            <select
                                id="departmentId"
                                name="departmentId"
                                value={formData.departmentId || ''}
                                onChange={handleChange}
                                className={errors.departmentId ? 'error' : ''}
                            >
                                <option value="">Select Department</option>
                                {dropdowns.departments?.map(d => (
                                    <option key={d.id} value={d.id}>{d.name}</option>
                                ))}
                            </select>
                            {errors.departmentId && <span className="error-text">{errors.departmentId}</span>}
                        </div>
                    </>
                );
            default:
                return null;
        }
    };

    return (
        <div className="entity-form-overlay">
            <div className="entity-form-modal">
                <h2>{entity ? 'Edit' : 'Add'} {entityType.slice(0, -1).charAt(0).toUpperCase() + entityType.slice(1, -1)}</h2>
                <form onSubmit={handleSubmit}>
                    {renderFields()}
                    <div className="form-actions">
                        <button type="button" onClick={onCancel} className="cancel-btn">Cancel</button>
                        <button type="submit" className="save-btn">Save</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default EntityForm;
