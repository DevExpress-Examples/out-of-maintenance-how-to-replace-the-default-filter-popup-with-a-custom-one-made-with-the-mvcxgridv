<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128579470/17.1.7%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T572362)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [HomeController.cs](./CS/MPG_CustomFilterPopup/Controllers/HomeController.cs)
* [CustomFilterItem.cs](./CS/MPG_CustomFilterPopup/Models/CustomFilterItem.cs)
* [_GridViewPartial.cshtml](./CS/MPG_CustomFilterPopup/Views/Home/_GridViewPartial.cshtml)
* [_PivotGridPartial.cshtml](./CS/MPG_CustomFilterPopup/Views/Home/_PivotGridPartial.cshtml)
* [Index.cshtml](./CS/MPG_CustomFilterPopup/Views/Home/Index.cshtml)
<!-- default file list end -->
# How to replace the default Filter Popup with a custom one made with the MVCxGridView


<p>This example demonstrates how to replace the default Filter Popup with a custom one made with the MVC Grid View extension. This solution demonstrates only a basic approach, and it is possible to customize it further to achieve a custom result. The whole sample functionality can be divided into three parts:</p>
<p>1. We use the <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.Mvc.PivotGridSettings.SetHeaderTemplateContent.overloads">PivotGridSettings.SetHeaderTemplateContent</a> method to assign a custom header template, where we replace the default filter button with a custom one created dynamically.</p>
<p> </p>
<p>2. To get information about the filter applied to a specific field, we use the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldBase_FilterValuestopic">PivotGridFieldBase.FilterValues</a> and <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldBase_GetUniqueValuestopic">PivotGridFieldBase.GetUniqueValues</a> properties. To pass filter information from the server to the client, we simply convert filter values to strings. This solution can be not enough in some situations. In this case, it might be necessary to update the code accordingly.</p>
<p> </p>
<p>3. To populate Grid View with data at runtime and apply the specified filter to the Pivot Grid extension, we use the <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.MVC.Scripts.MVCxClientPivotGrid.PerformCallback.overloads">MVCxClientPivotGrid.PerformCallback</a> and <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.MVC.Scripts.MVCxClientGridView.PerformCallback.overloads">MVCxClientGridView.PerformCallback</a> methods. The client-side row selection functionality is provided by the Grid View's built-in <a href="http://documentation.devexpress.com/#AspNet/CustomDocument3737">Selection</a> feature.<br><br><strong><br>See Also: <br></strong><a href="https://www.devexpress.com/Support/Center/p/E4669">E4669: How to replace the default Filter Popup with a custom one made with ASPxGridView control</a></p>

<br/>


