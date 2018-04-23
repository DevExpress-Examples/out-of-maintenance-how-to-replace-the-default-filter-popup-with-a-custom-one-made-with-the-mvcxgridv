using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.ASPxPivotGrid;

namespace MPG_CustomFilterPopup.Models {
    public class CustomFilterItem {

        public object Value { get; set; }
        public string DisplayText { get; set; }
        public bool Selected { get; set; }

        public override string ToString() {
            return DisplayText + ": " + Selected;
        }

        public static CustomFilterItem[] GetItems(DevExpress.Web.ASPxPivotGrid.PivotGridField field) {
            var selectedValues = field.FilterValues.ValuesIncluded;
            return field.GetUniqueValues()
           .Select(v => new MPG_CustomFilterPopup.Models.CustomFilterItem() { Value = v, DisplayText = field.GetDisplayText(v), Selected = ContainItem(selectedValues, v) })
           .ToArray();            
        }

        internal static object[] GetValuesExcluded(PivotGridField field, string[] values) {
            values = values.OrderBy(v => v).ToArray();
            var availableItems = ((Dictionary<string, CustomFilterItem[]>)HttpContext.Current.Session["pivotFilterValues"])[field.ID];
            //object[] uniqueValues = field.GetUniqueValues();
            object[] valuesExcluded = availableItems.Select(v => v.Value).Where(v => !ContainItem(values, Convert.ToString(v))).ToArray();
            return valuesExcluded;
        }

        static bool ContainItem(object[] values, object target) {
            int left = 0;
            int right = values.Length - 1;
            while (left <= right) {
                int middle = (left + right) / 2;
                int result = ((IComparable)target).CompareTo((IComparable)values[middle]);
                if (result == 0)
                    return true;
                else {
                    if (result < 0)
                        right = middle - 1;
                    else
                        left = middle + 1;
                }

            }
            return false;
        }


    }
}