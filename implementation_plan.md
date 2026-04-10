# Hierarchical Week and Day Selection for Schedules

This plan details how to update the "Add New Schedule" form in the Data Management UI to allow users to select a week first, and then a specific day within that week.

## User Review Required

> [!IMPORTANT]
> The current "Add New Schedule" form has separate fields for "Day of Week" and "Specific Date". This plan proposes replacing these with a unified "Week" and "Day" hierarchical selector.
> - **Recurring Schedules**: The user can choose to leave the date blank if they want it to repeat every week (as specified in current implementation), or we can provide a "Weekly Recurring" option in the week selector.
> - **Date Range**: The week list will be generated based on the current year (2026), matching the display logic in the main schedule view.

## Proposed Changes

### [Frontend Components]

---

#### [NEW] [dateUtils.js](file:///f:/source/repos/Schedule%20Manager/frontend/src/services/dateUtils.js)
Extract the week generation logic from `App.js` into a shared utility to ensure consistency across the application.

- `getWeeks(year)`: Generates 52 weeks for a given year.
- `getDaysInWeek(week)`: Generates 7 days (Monday-Sunday) for a given week object.
- `getDayName(dayOfWeek)`: Returns "Thứ 2", "Thứ 3", etc.

#### [MODIFY] [App.js](file:///f:/source/repos/Schedule%20Manager/frontend/src/App.js)
Update `App.js` to use the new `dateUtils.js` for week generation.

#### [MODIFY] [ScheduleForm.js](file:///f:/source/repos/Schedule%20Manager/frontend/src/components/ScheduleForm.js)
Integrate the hierarchical picker:

- Add logic to generate a "Week" `<select>` and a "Day" `<select>`.
- Selection in "Week" updates the options in "Day".
- Selection updates `formData.date` and `formData.dayOfWeek`.
- Handle "Edit" mode to pre-fill the correct week/day based on existing data.

## Open Questions

- **Week Range Selection**: Should we show all 52 weeks centered around the current week?
- **Recurring Schedules**: Should we keep the "Day of Week" selector as-is for recurring schedules, and only use the week/day logic when picking a specific date?

## Verification Plan

### Manual Verification
1.  Navigate to **Management** > **Schedules**.
2.  Click **+ Add Schedule**.
3.  Ensure the "Day of Week" and "Specific Date" inputs are replaced by a **Week** dropdown and a **Day** dropdown.
4.  Confirm that selecting a **Week** populates the **Day** dropdown with the correct dates (e.g., Week 15: Monday April 13, Tuesday April 14, ...).
5.  Select a specific date and save.
6.  Verify that the schedule is created with both the correct `Date` and `DayOfWeek`.
