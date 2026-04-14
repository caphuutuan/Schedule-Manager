import React, { useState, useEffect, useCallback, useMemo } from 'react';
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

const DAY_NAMES = { 1: 'Thứ 2', 2: 'Thứ 3', 3: 'Thứ 4', 4: 'Thứ 5', 5: 'Thứ 6', 6: 'Thứ 7', 7: 'Chủ Nhật' };

const Management = ({ school }) => {
    const [activeTab, setActiveTab] = useState('classes');
    const [entities, setEntities] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showForm, setShowForm] = useState(false);
    const [editingEntity, setEditingEntity] = useState(null);
    const [dropdowns, setDropdowns] = useState({ departments: [], schools: [] });

    // Sort & Filter state
    const [sortField, setSortField] = useState('');
    const [sortDir, setSortDir] = useState('asc');
    const [filterGrade, setFilterGrade] = useState('');
    const [filterDept, setFilterDept] = useState('');
    const [filterSchedule, setFilterSchedule] = useState('');
    const [filterWeek, setFilterWeek] = useState('');
    const [searchText, setSearchText] = useState(''); // universal text search

    // Reset sort/filter when switching tabs
    useEffect(() => {
        setSortField('');
        setSortDir('asc');
        setFilterGrade('');
        setFilterDept('');
        setFilterSchedule('');
        setFilterWeek('');
        setSearchText('');
    }, [activeTab]);

    const fetchEntities = useCallback(async () => {
        setLoading(true);
        try {
            let data;
            if (activeTab === 'schedules') {
                data = await api.getSchedules(school.id, { schoolId: school.id });
            } else if (activeTab === 'schools') {
                data = await api.getSchools();
            } else {
                data = await api[entityConfig[activeTab].api](school.id);
            }
            setEntities(data);
        } catch (error) {
            console.error('Error fetching entities:', error);
            alert('Failed to fetch entities');
        } finally {
            setLoading(false);
        }
    }, [activeTab, school.id]);

    const fetchInitialData = useCallback(async () => {
        try {
            const deps = await api.getDepartments(school.id);
            setDropdowns({ departments: deps });
        } catch (error) {
            console.error('Error fetching initial data:', error);
        }
    }, [school.id]);

    useEffect(() => {
        fetchEntities();
        fetchInitialData();
    }, [fetchEntities, fetchInitialData]);

    const handleSort = (field) => {
        if (sortField === field) {
            setSortDir(d => d === 'asc' ? 'desc' : 'asc');
        } else {
            setSortField(field);
            setSortDir('asc');
        }
    };

    const sortIcon = (field) => {
        if (sortField !== field) return <span className="sort-icon neutral">⇅</span>;
        return <span className="sort-icon active">{sortDir === 'asc' ? '↑' : '↓'}</span>;
    };

    const displayedEntities = useMemo(() => {
        let list = [...entities];

        // ─── Dropdown/specific filters ───────────────────────────────────────
        if (activeTab === 'classes' && filterGrade) {
            list = list.filter(e => String(e.grade) === filterGrade);
        }
        if ((activeTab === 'teachers' || activeTab === 'subjects') && filterDept) {
            list = list.filter(e => String(e.departmentId) === filterDept);
        }
        if (activeTab === 'schedules') {
            if (filterWeek) {
                list = list.filter(e => String(e.weekNumber) === filterWeek);
            }
            if (filterSchedule) {
                const q = filterSchedule.toLowerCase();
                list = list.filter(e =>
                    e.subjectName?.toLowerCase().includes(q) ||
                    e.className?.toLowerCase().includes(q) ||
                    e.teacherName?.toLowerCase().includes(q)
                );
            }
        }

        // ─── Universal text search (all tabs) ────────────────────────────────
        if (searchText) {
            const q = searchText.toLowerCase();
            list = list.filter(e => {
                if (activeTab === 'schedules') {
                    return (
                        e.subjectName?.toLowerCase().includes(q) ||
                        e.className?.toLowerCase().includes(q) ||
                        e.teacherName?.toLowerCase().includes(q)
                    );
                }
                // For classes, teachers, subjects, departments, schools — search by name
                return e.name?.toLowerCase().includes(q);
            });
        }

        // ─── Sort (locale-aware for Vietnamese text) ─────────────────────────
        if (sortField) {
            list.sort((a, b) => {
                let va = a[sortField];
                let vb = b[sortField];
                // Numeric fields
                if (typeof va === 'number' && typeof vb === 'number') {
                    return sortDir === 'asc' ? va - vb : vb - va;
                }
                // String: locale-aware so Vietnamese diacritics sort correctly
                const sa = va != null ? String(va) : '';
                const sb = vb != null ? String(vb) : '';
                const cmp = sa.localeCompare(sb, 'vi', { sensitivity: 'base' });
                return sortDir === 'asc' ? cmp : -cmp;
            });
        }

        return list;
    }, [entities, activeTab, filterGrade, filterDept, filterSchedule, filterWeek, searchText, sortField, sortDir]);

    const gradeOptions = useMemo(() =>
        [...new Set(entities.map(e => e.grade))].filter(Boolean).sort((a, b) => a - b),
        [entities]
    );

    const handleAdd = () => { setEditingEntity(null); setShowForm(true); };
    const handleEdit = (entity) => { setEditingEntity(entity); setShowForm(true); };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this item?')) {
            try {
                if (activeTab === 'schools') {
                    await api.deleteSchool(id);
                } else {
                    await api[entityConfig[activeTab].delete](school.id, id);
                }
                fetchEntities();
            } catch (error) {
                console.error('Error deleting entity:', error);
                alert('Failed to delete entity');
            }
        }
    };

    const handleSave = async (formData) => {
        try {
            const isSchool = activeTab === 'schools';
            if (editingEntity) {
                if (isSchool) await api.updateSchool(editingEntity.id, formData);
                else await api[entityConfig[activeTab].update](school.id, editingEntity.id, formData);
            } else {
                if (isSchool) await api.createSchool(formData);
                else await api[entityConfig[activeTab].create](school.id, formData);
            }
            setShowForm(false);
            fetchEntities();
        } catch (error) {
            console.error('Error saving entity:', error);
            alert('Failed to save entity');
        }
    };

    const renderFilterBar = () => {
        const countLabel = {
            classes: 'lớp',
            teachers: 'giáo viên',
            subjects: 'môn học',
            departments: 'tổ bộ môn',
            schools: 'trường',
            schedules: 'tiết học',
        }[activeTab] || 'bản ghi';

        return (
            <div className="filter-bar">
                {/* Universal text search */}
                <div className="filter-search-wrap">
                    <span className="filter-search-icon">🔍</span>
                    <input
                        className="filter-input"
                        type="text"
                        placeholder={activeTab === 'schedules'
                            ? 'Tìm môn học, lớp, giáo viên...'
                            : 'Tìm theo tên...'}
                        value={searchText}
                        onChange={e => setSearchText(e.target.value)}
                    />
                    {searchText && (
                        <button className="filter-clear-btn" onClick={() => setSearchText('')}>✕</button>
                    )}
                </div>

                {/* Grade filter for Classes */}
                {activeTab === 'classes' && (
                    <>
                        <span className="filter-divider" />
                        <label className="filter-label">Khối:</label>
                        <select className="filter-select" value={filterGrade} onChange={e => setFilterGrade(e.target.value)}>
                            <option value="">Tất cả</option>
                            {gradeOptions.map(g => <option key={g} value={g}>Khối {g}</option>)}
                        </select>
                    </>
                )}

                {/* Department filter for Teachers & Subjects */}
                {(activeTab === 'teachers' || activeTab === 'subjects') && (
                    <>
                        <span className="filter-divider" />
                        <label className="filter-label">Tổ bộ môn:</label>
                        <select className="filter-select" value={filterDept} onChange={e => setFilterDept(e.target.value)}>
                            <option value="">Tất cả</option>
                            {dropdowns.departments.map(d => <option key={d.id} value={d.id}>{d.name}</option>)}
                        </select>
                    </>
                )}

                {/* Schedule-specific search (subject/class/teacher combined) and Week filter */}
                {activeTab === 'schedules' && (
                    <>
                        <span className="filter-divider" />
                        <label className="filter-label">Tuần:</label>
                        <select className="filter-select" value={filterWeek} onChange={e => setFilterWeek(e.target.value)}>
                            <option value="">Tất cả</option>
                            {[...Array(35)].map((_, i) => <option key={i+1} value={i+1}>Tuần {i+1}</option>)}
                        </select>
                        <span className="filter-divider" />
                        <input
                            className="filter-input"
                            type="text"
                            placeholder="Lọc thêm môn, lớp, GV..."
                            value={filterSchedule}
                            onChange={e => setFilterSchedule(e.target.value)}
                            style={{ maxWidth: 200 }}
                        />
                    </>
                )}

                <span className="filter-count">{displayedEntities.length} {countLabel}</span>
            </div>
        );
    };

    const renderTable = () => {
        if (loading) return <div className="loading">Loading...</div>;

        const SortTh = ({ field, children, align }) => (
            <th
                className={`sortable-th ${align === 'right' ? 'actions-header' : ''}`}
                onClick={() => handleSort(field)}
                style={{ cursor: 'pointer', userSelect: 'none' }}
            >
                <span className="th-content">{children}{sortIcon(field)}</span>
            </th>
        );

        return (
            <div className="table-container">
                {renderFilterBar()}
                <table>
                    <thead>
                        <tr>
                            <th>ID</th>
                            {activeTab === 'schedules' ? (
                                <>
                                    <SortTh field="subjectName">Môn học</SortTh>
                                    <SortTh field="className">Lớp</SortTh>
                                    <SortTh field="teacherName">Giáo viên</SortTh>
                                    <SortTh field="weekNumber">Tuần</SortTh>
                                    <SortTh field="semester">Học kỳ</SortTh>
                                    <SortTh field="dayOfWeek">Thứ</SortTh>
                                    <SortTh field="period">Tiết</SortTh>
                                </>
                            ) : (
                                <SortTh field="name">Tên</SortTh>
                            )}
                            {activeTab === 'classes' && <SortTh field="grade">Khối</SortTh>}
                            {(activeTab === 'teachers' || activeTab === 'subjects') && (
                                <SortTh field="departmentName">Tổ bộ môn</SortTh>
                            )}
                            <th className="actions-header">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {displayedEntities.length > 0 ? (
                            displayedEntities.map((item) => (
                                <tr key={item.id}>
                                    <td className="id-cell">{item.id}</td>
                                    {activeTab === 'schedules' ? (
                                        <>
                                            <td className="entity-name">{item.subjectName}</td>
                                            <td>{item.className}</td>
                                            <td>{item.teacherName}</td>
                                            <td>{item.weekNumber ? `Tuần ${item.weekNumber}` : 'Cố định'}</td>
                                            <td>{item.semester ? `HK${item.semester}` : '-'}</td>
                                            <td>
                                                <span className="day-badge">
                                                    {DAY_NAMES[item.dayOfWeek] || `DoW ${item.dayOfWeek}`}
                                                </span>
                                            </td>
                                            <td>
                                                <span className="period-pill">Tiết {item.period}</span>
                                            </td>
                                        </>
                                    ) : (
                                        <td className="entity-name">{item.name}</td>
                                    )}
                                    {activeTab === 'classes' && (
                                        <td><span className="grade-badge">Khối {item.grade}</span></td>
                                    )}
                                    {(activeTab === 'teachers' || activeTab === 'subjects') && (
                                        <td className="dept-cell">{item.departmentName || item.departmentId}</td>
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
                                <td colSpan="8" className="empty-state">Không tìm thấy dữ liệu</td>
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
                        school={school}
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
                        schoolLevel={school.level}
                    />
                )
            )}
        </div>
    );
};

export default Management;
