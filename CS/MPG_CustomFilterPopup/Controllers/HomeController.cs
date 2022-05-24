using DevExpress.Web;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Web.Mvc;
using DevExpress.XtraPivotGrid;
using MPG_CustomFilterPopup.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MPG_CustomFilterPopup.Controllers {
    public class HomeController : Controller {
        // GET: Home
        public ActionResult Index() {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult PivotGridPartial() {
            ViewBag.PivotGridSettings = GetPivotGridSettings();
            var model = new NwindDataClassesDataContext().Invoices;
            return PartialView("_PivotGridPartial", model);
        }


        [ValidateInput(false)]
        public ActionResult PivotGridCustomPartial(string FieldID, string[] Values) {
            var settigns = GetPivotGridSettings();
            settigns.BeforePerformDataSelect = (s, e) => {
                var pivot = (MVCxPivotGrid)s;
                var field = pivot.Fields[FieldID];

                field.FilterValues.ValuesExcluded = CustomFilterItem.GetValuesExcluded(field, Values);
                Debug.WriteLine(string.Join("; ", Values));
            };
            ViewBag.PivotGridSettings = settigns;
            var model = new NwindDataClassesDataContext().Invoices;
            return PartialView("_PivotGridPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult GVFilterItemsPartial() {
            ViewBag.GVFilterItemsSettings = GetGVFilterItemsSettings();

            var model = Session["GridDataSource"];
            return PartialView("_GridViewPartial", model);
        }
        public ActionResult GVFilterItemsCustomPartial(string Action, string FieldID) {
            var settings = GetGVFilterItemsSettings();
            CustomFilterItem[] model = null;
            if (Action == "Close") {
                Session["GridDataSource"] = null;
            }            
            else if (Action == "Show" && !string.IsNullOrEmpty(FieldID)) {
                model = ((Dictionary<string, CustomFilterItem[]>)Session["pivotFilterValues"])[FieldID];
                Session["GridDataSource"] = model;
                settings.BeforeGetCallbackResult = (s, e) => {
                    MVCxGridView gv = (MVCxGridView)s;
                    gv.Selection.BeginSelection();
                    foreach (var item in model)
                        gv.Selection.SetSelectionByKey(item.Value, item.Selected);
                    gv.Selection.EndSelection();
                    gv.JSProperties["cpFieldID"] = FieldID;
                };
            }
            else if (Action == "Invert") {
                model = (CustomFilterItem[])Session["GridDataSource"];
                settings.BeforeGetCallbackResult = (s, e) => {
                    MVCxGridView gv = (MVCxGridView)s;
                    List<object> selectedValues = gv.GetSelectedFieldValues(new string[] { "Value" });
                    gv.Selection.SelectAll();
                    foreach (object val in selectedValues)
                        gv.Selection.UnselectRowByKey(val);

                };
            }            
            ViewBag.GVFilterItemsSettings = settings;
            return PartialView("_GridViewPartial", model);
        }

        private PivotGridSettings GetPivotGridSettings() {
            var settings = new PivotGridSettings();
            settings.Name = "PivotGrid";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "PivotGridPartial" };
            settings.CustomActionRouteValues = new { Controller = "Home", Action = "PivotGridCustomPartial" };
            settings.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.Optimized;
            settings.Fields.Add(field => {
                field.ID = "fieldCountry";
                field.Area = PivotArea.RowArea;
                field.DataBinding = new DataSourceColumnBinding("Country");
                field.Caption = "Country";
            });
            settings.Fields.Add(field => {
                field.ID = "fieldOrderDate";
                field.Area = PivotArea.ColumnArea;
                field.DataBinding = new DataSourceColumnBinding("OrderDate", PivotGroupInterval.DateYear);
                field.Caption = "OrderDate";
            });
            settings.Fields.Add(field => {
                field.ID = "fieldProductName";
                field.Area = PivotArea.FilterArea;
                field.DataBinding = new DataSourceColumnBinding("ProductName");
                field.Caption = "ProductName";
            });
            settings.Fields.Add(field => {
                field.ID = "fieldExtendedPrice";
                field.Area = PivotArea.DataArea;
                field.DataBinding = new DataSourceColumnBinding("ExtendedPrice");
                field.Caption = "ExtendedPrice";
            });

            settings.PreRender = settings.GridLayout = (s, e) => {
                var pivot = (MVCxPivotGrid)s;
                Session["pivotFilterValues"] = pivot.Fields.OfType<PivotGridField>().Where(f => f.Visible && f.Area != PivotArea.DataArea).ToDictionary(f => f.ID, f => CustomFilterItem.GetItems(f));                
            };
            return settings;

        }

        public GridViewSettings GetGVFilterItemsSettings() {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "gvFilterItems";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "GVFilterItemsPartial" };
            settings.CustomActionRouteValues = new { Controller = "Home", Action = "GVFilterItemsCustomPartial" };

            settings.KeyFieldName = "Value";

            settings.SettingsPager.PageSize = 15;

            settings.SettingsPager.Visible = true;
            settings.Settings.ShowGroupPanel = false;
            settings.Settings.ShowFilterRow = true;
            settings.Settings.GridLines = System.Web.UI.WebControls.GridLines.None;
            settings.Styles.SelectedRow.BackColor = System.Drawing.Color.White;
            settings.Styles.SelectedRow.Font.Bold = true;

            settings.SettingsBehavior.AllowSelectByRowClick = false;
            settings.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            

            settings.CommandColumn.Visible = true;
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
            settings.Columns.Add("DisplayText");
            return settings;
        }




    }


}