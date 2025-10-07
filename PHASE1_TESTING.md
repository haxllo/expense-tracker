# Phase 1 Testing Checklist ✅

## Before Testing

```bash
cd C:\Projects\expence-tracker\ExpenseTracker
dotnet build
dotnet run
```

---

## 🎯 Phase 1.1 - Charts & Analytics Testing

### Open Reports Window
- [ ] Click **📊 Reports** button on main dashboard
- [ ] Reports window opens successfully
- [ ] No errors in console

### Test Pie Chart (Category Breakdown)
- [ ] Pie chart displays with data
- [ ] Each slice shows category name
- [ ] Data labels show amounts (₹X)
- [ ] Legend appears on the right
- [ ] All categories are visible
- [ ] Colors are distinct and clear

### Test Bar Chart (Monthly Trends)
- [ ] Bar chart shows up to 6 months of data
- [ ] X-axis shows month names (e.g., "Jan 2025")
- [ ] Y-axis shows amounts
- [ ] Bars are color-coded (blue)
- [ ] Data labels on top of bars
- [ ] Grid lines visible for easy reading

### Test Line Chart (Daily Pattern)
- [ ] Line chart shows last 30 days
- [ ] X-axis shows dates (MM/DD format)
- [ ] Line is smooth and green
- [ ] Circular markers on data points
- [ ] Zero days show as flat line at bottom

### Test Summary Cards
- [ ] **Total Expenses** shows correct amount
- [ ] **Daily Average** calculates correctly
- [ ] **Top Category** displays the highest spending category
- [ ] All amounts are formatted with ₹ symbol

### Test Date Range Filter
- [ ] Change "From" date picker
- [ ] Change "To" date picker
- [ ] Charts update automatically
- [ ] Data reflects the selected date range
- [ ] Summary cards update

### Test Refresh Button
- [ ] Click **🔄 Refresh** button
- [ ] All charts reload
- [ ] Data is current
- [ ] No errors

### Test Close Button
- [ ] Click **✖ Close** button
- [ ] Reports window closes
- [ ] Returns to main dashboard
- [ ] Main window still responsive

---

## 🎯 Phase 1.2 - Dashboard Enhancements Testing

### Test Dashboard Cards (5 Total)

#### 1. Total Expenses Card
- [ ] Shows all-time total
- [ ] Pink/Red color
- [ ] Amount formatted as ₹X (no decimals)
- [ ] Updates when adding new expense

#### 2. This Month Card
- [ ] Shows current month total
- [ ] Green color
- [ ] Displays trend indicator (↑ or ↓)
- [ ] Trend percentage shown
- [ ] Trend color: Red for increase, Green for decrease

**Test Trend Indicator:**
- [ ] If you spent MORE this month: Shows **↑ X%** in RED
- [ ] If you spent LESS this month: Shows **↓ X%** in GREEN
- [ ] If same as last month: Shows **→ 0%** in GRAY
- [ ] First month of data: Shows **"New"** in BLUE

#### 3. Last 7 Days Card
- [ ] Shows weekly total (last 7 days)
- [ ] Blue color
- [ ] Updates daily
- [ ] Correct calculation

#### 4. Avg Daily (Month) Card
- [ ] Shows average daily spending for current month
- [ ] Orange color
- [ ] Formula: Monthly Total ÷ Days in month so far
- [ ] Updates when adding expenses

#### 5. Top Category Card
- [ ] Shows highest spending category for current month
- [ ] Purple color
- [ ] Category name displayed
- [ ] Changes when different category becomes top
- [ ] Shows "N/A" if no data

### Test Card Updates
- [ ] Add a new expense
- [ ] All cards update immediately (without restart)
- [ ] Amounts are correct
- [ ] No need to click refresh

### Test Responsive Layout
- [ ] All 5 cards fit on one row
- [ ] Cards are evenly spaced
- [ ] Text is readable
- [ ] No overflow or wrapping issues

---

## 🔄 Integration Testing

### Add Expense Flow
1. [ ] Click **➕ Add Expense**
2. [ ] Fill in details (Amount, Category, Date)
3. [ ] Click **💾 Save**
4. [ ] Window closes
5. [ ] Main dashboard updates immediately
6. [ ] New expense appears in list
7. [ ] All 5 cards update with new totals
8. [ ] Open Reports → Charts show new data

### Edit Expense Flow
1. [ ] Select an expense from list
2. [ ] Click **✏️ Edit**
3. [ ] Change amount or category
4. [ ] Click **💾 Save**
5. [ ] Dashboard updates
6. [ ] Cards reflect new amount
7. [ ] Charts update when reports opened

### Delete Expense Flow
1. [ ] Select an expense
2. [ ] Click **🗑️ Delete**
3. [ ] Expense removed from list
4. [ ] Cards update immediately
5. [ ] Totals recalculate correctly

### Filter & Search
1. [ ] Change date range filter
2. [ ] Expense list updates
3. [ ] Cards recalculate for date range
4. [ ] Search by keyword
5. [ ] Results filter correctly

---

## 🐛 Common Issues to Check

### Dashboard Issues
- [ ] No "N/A" or blank values (unless no data)
- [ ] No negative totals
- [ ] Trend indicator displays correctly
- [ ] All amounts use ₹ symbol
- [ ] No decimal places showing (should be ₹1,234 not ₹1,234.56)

### Reports Issues
- [ ] Charts render properly (not blank)
- [ ] No "0" or empty charts if data exists
- [ ] Colors are visible and distinct
- [ ] Labels don't overlap
- [ ] Window can be resized
- [ ] Close button works

### Performance Issues
- [ ] Dashboard loads in < 2 seconds
- [ ] Reports open in < 3 seconds
- [ ] No lag when adding expenses
- [ ] Smooth scrolling in reports
- [ ] No memory leaks after multiple operations

---

## 📊 Test Data Scenarios

### Scenario 1: New User (No Data)
- [ ] Dashboard shows ₹0 for all cards
- [ ] Top Category shows "N/A"
- [ ] Trend shows "New"
- [ ] Reports show empty charts or message
- [ ] No errors

### Scenario 2: One Month of Data
- [ ] All cards show data
- [ ] Trend shows "New" or percentage
- [ ] Charts show data points
- [ ] Categories distributed in pie chart

### Scenario 3: Multiple Months
- [ ] Monthly trend comparison works
- [ ] Bar chart shows multiple months
- [ ] Trend indicator accurate
- [ ] Daily average calculated correctly

### Scenario 4: Large Dataset (50+ expenses)
- [ ] Dashboard responsive
- [ ] Charts render properly
- [ ] Scrolling works in expense list
- [ ] Performance acceptable

---

## ✅ Phase 1 Success Criteria

All features working:
- [x] 5 Dashboard summary cards
- [x] Trend indicator (↑↓ %)
- [x] 3 Charts (Pie, Bar, Line)
- [x] Date range filtering
- [x] Real-time updates
- [x] Close button on Reports
- [x] No build errors
- [x] No runtime errors
- [x] Good performance

---

## 🎉 If All Tests Pass

Phase 1 is complete! You're ready for:
- **Phase 2: Budget Management**

## 🐛 If Issues Found

Note any issues here:
- Issue 1: _____________________________
- Issue 2: _____________________________
- Issue 3: _____________________________

---

**Testing Date:** _____________
**Tester:** _____________
**Result:** ⬜ Pass ⬜ Fail ⬜ Needs Fixes
