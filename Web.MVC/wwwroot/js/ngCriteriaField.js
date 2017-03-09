function CriteriaField(field, caption) {
    var objField = {};
    objField.field = field;
    objField.caption = caption;

    objField.isDisplay = true;
    objField.isValueFilter = true;
    objField.isRangeFilter = true;
    objField.isGroupBy = true;
    objField.isOrderBy = true;

    objField.isDisplayChecked = false;
    objField.isDisplayDisabled = false;

    objField.isFilterChecked = false;
    objField.isFilterDisabled = false;

    objField.filterControlType = "text";

    objField.filterControlDisabled = false;
    objField.filterControlValue = "";

    objField.filterControlFromDisabled = false;
    objField.filterControlFromValue = "";

    objField.filterControlToDisabled = false;
    objField.filterControlToValue = "";

    objField.isGroupChecked = false;            //to keep track of control behaviuor
    objField.isGroupDisabled = false;

    objField.isOrderChecked = false;            //to keep track of control behaviuor
    objField.isOrderDisabled = false;
    return objField;
};
