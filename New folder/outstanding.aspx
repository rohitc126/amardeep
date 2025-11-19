<%@ Page Title="Collection Module - OS Receivable" Language="VB" MasterPageFile="~/spadmin/MasterAdmin.master" AutoEventWireup="false" CodeFile="outstanding.aspx.vb" Inherits="spadmin_outstanding" %>
<%@ Register Src="~/spadmin/dashboard-filters.ascx" TagName="dashboard_filters" TagPrefix="uc"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Collection Module : OS Receivable</title>

    <%--Script for dg Pager control & DG Customization Window--%>
    <script type="text/javascript">
        // <![CDATA[
        var CustomPager = {
            gotoBox_Init: function (s, e) {
                s.SetText(1 + dg.GetPageIndex());
            },
            gotoBox_KeyPress: function (s, e) {
                if (e.htmlEvent.keyCode != 13)
                    return;
                e.htmlEvent.cancelBubble = true;
                e.htmlEvent.returnValue = false;
                CustomPager.applyGotoBoxValue(s);
            },
            gotoBox_ValueChanged: function (s, e) {
                CustomPager.applyGotoBoxValue(s);
            },
            applyGotoBoxValue: function (textBox) {
                var pageIndex = parseInt(textBox.GetText()) - 1;
                if (pageIndex < 0)
                    pageIndex = 0;
                dg.GotoPage(pageIndex);
            },
            combo_SelectedIndexChanged: function (s, e) {
                dg.PerformCallback(s.GetSelectedItem().text + '|PagerCombo');
            }
        };
        // ]]>

        // Function to handle tab navigation
        function handleTabNavigation(e) {
            var target = e.target || e.srcElement;

            // If the pressed key is the "Tab" key (keyCode 9)
            if (e.keyCode === 9) {
                // Prevent the default tabbing behavior
                e.preventDefault();

                // Find the next valid focusable element, ignoring readonly and disabled controls
                var nextFocusable = getNextFocusableElement(target, e.shiftKey);

                // Focus the next valid element
                if (nextFocusable) {
                    nextFocusable.focus();
                }
            }
            if (e.keyCode === 13) {
                // Check if the target element is a button (or another type of element you want to trigger the action)
                if (target && (target.tagName === 'BUTTON' || target.type === 'submit' || target.type === 'button')) {
                    return; // Allow Enter to trigger the button action
                }
                // Prevent the default form submission behavior if the focus is not on a button or submit input
                e.preventDefault();
            } 
        }

        // Function to find the next or previous focusable element based on tab direction
        function getNextFocusableElement(currentElement, isShiftTab) {
            // Get all focusable, editable input/select/textarea elements that are neither readonly nor disabled
            var elements = Array.from(document.querySelectorAll(
                "input:not([readonly]):not([disabled]):not([type='hidden']), " +
                "select:not([readonly]):not([disabled]):not([hidden]), " +
                "textarea:not([readonly]):not([disabled]):not([hidden]), " +
                "input[type='checkbox']:not([disabled]), " +
                "input[type='radio']:not([disabled]), " +
                "input[type='date']:not([readonly]):not([disabled])"
            ));

            // Find the index of the current focused element in the list
            var currentIndex = elements.indexOf(currentElement);

            if (currentIndex === -1) return null; // If the element isn't found, return null

            // Move forward or backward based on Shift + Tab
            if (isShiftTab) {
                // For Shift + Tab (move backward)
                if (currentIndex > 0) {
                    return elements[currentIndex - 1];
                }
            } else {
                // For Tab (move forward)
                if (currentIndex < elements.length - 1) {
                    return elements[currentIndex + 1];
                }
            }

            return null; // Return null if no more focusable elements are found
        }

        // Function to select the text inside input fields (for text-based controls)
        function selectTextOnFocus(e) {
            var target = e.target || e.srcElement;

            // If it's a text input field (text, email, search, etc.)
            if (target && (target.type === "text" || target.type === "email" || target.type === "search" || target.tagName.toLowerCase() === "textarea")) {
                target.select(); // Select the text inside the input
            }
        }

        // Add event listener for the "Tab" key on page load
        window.onload = function () {
            // Listen for "Tab" key press to handle tab navigation
            document.body.addEventListener('keydown', handleTabNavigation);

            // Listen for focus on all input elements to select text when focused
            document.body.addEventListener('focus', selectTextOnFocus, true);
        };

        // Keyboard Shortcut Key works only when "KeyboardSupport" property of "dg" set to "true"
        function OnInit(s, e) {
            ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "keydown",
                function (evt) {
                    switch (evt.keyCode) {
                        //F2            
                        case 113:
                            var key = s.GetFocusedRowIndex();
                            s.StartEditRow(key);

                            break;
                    }
                }
            );
        }

        // Customization Field Window
        function btnColumnChooser_Click(s, e) {
            if (dg.IsCustomizationWindowVisible())
                dg.HideCustomizationWindow();
            else
                dg.ShowCustomizationWindow();
        }

        function OnContextMenuItemClick(sender, e) {
            if (e.item.name == "Refresh") {
                location.reload();
            }
        }
    </script>
    <%--Script for dg Pager control & DG Customization Window--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodytext" runat="Server">
    <%--<uc:dashboard_filters id="dashboard_filters" runat="server"></uc:dashboard_filters>--%>

    <div class="row">
        <div class="col-md-12">
            <div class="white-box user-table">
                <div class="table-responsive">
                    <section class="bg-white-border" >
                        <div class="button-container p-b-5">
                            <div class="m-l-15">
                                <div class="col-lg-2 col-sm-4 col-xs-12">
                                    <dx:ASPxButton id="btnShowSearchPanel" runat="server" ToolTip="Show / Hide Common Search" 
                                        OnClick="btnShowSearchPanel_Click" UseSubmitBehavior="False" AutoPostBack="false" cssclass="no-border" >
                                        <image url="~/images/search.png" width="30px" ToolTip="Show / Hide Common Search" />
                                        <backgroundimage imageurl="~/images/white.png" repeat="NoRepeat" />
                                        <border borderstyle="None" /> 
                                    </dx:ASPxButton>
                                </div>
                                <div class="col-lg-2 col-sm-4 col-xs-12">
                                    <dx:ASPxButton id="btnShowFilter" runat="server" ToolTip="Show / Hide Filter Options" 
                                        OnClick="btnShowFilter_Click" UseSubmitBehavior="False" AutoPostBack="false" cssclass="no-border" >
                                        <image url="~/images/filter.png" width="30px" ToolTip="Show / Hide Filter Options" />
                                        <backgroundimage imageurl="~/images/white.png" repeat="NoRepeat" />
                                        <border borderstyle="None" /> 
                                    </dx:ASPxButton>
                                </div>
                                <div class="col-lg-2 col-sm-4 col-xs-12">
                                    <dx:ASPxButton ID="btnShowGroupPanel" runat="server" ToolTip="Show / Hide Group Options" 
                                        OnClick="btnShowGroupPanel_Click" UseSubmitBehavior="False" AutoPostBack="false" cssclass="no-border" >
                                        <image url="~/images/group.png" width="30px" />
                                        <backgroundimage imageurl="~/images/white.png" repeat="NoRepeat" />
                                        <border borderstyle="None" />
                                    </dx:ASPxButton>
                                </div>
                            </div>
                            <div class="righ-panel m-r-15">
                                <div class="col-lg-2 col-sm-4 col-xs-12">
                                    <dx:ASPxButton ID="btnXlsxExport" runat="server" ToolTip="Export to Excel" 
                                        OnClick="btnXlsxExport_Click" UseSubmitBehavior="False" AutoPostBack="false" cssclass="no-border" >
                                        <image url="~/images/xls.png" width="30px" />
                                        <backgroundimage imageurl="~/images/white.png" repeat="NoRepeat" />
                                        <border borderstyle="None" />
                                    </dx:ASPxButton>
                                </div>
                                <div class="col-lg-2 col-sm-4 col-xs-12">
                                    <dx:ASPxButton ID="btnColumnChooser" runat="server" ToolTip="Column Chooser" 
                                        UseSubmitBehavior="False" AutoPostBack="false" cssclass="no-border" >
                                        <image url="~/images/choosing.png" width="30px" />
                                        <backgroundimage imageurl="~/images/white.png" repeat="NoRepeat" />
                                        <border borderstyle="None" />
                                        <ClientSideEvents Click="btnColumnChooser_Click" />
                                    </dx:ASPxButton>
                                </div>
                                <div class="col-lg-2 col-sm-4 col-xs-12">
                                    <dx:ASPxButton ID="btnRefresh" runat="server" ToolTip="Refresh Page" 
                                        OnClick="btnRefresh_Click" UseSubmitBehavior="False" AutoPostBack="false" cssclass="no-border">
                                        <image url="~/images/refresh.png" width="30px" />
                                        <backgroundimage imageurl="~/images/white.png" repeat="NoRepeat" />
                                        <border borderstyle="None" />
                                    </dx:ASPxButton>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-14">
                            <dx:ASPxHint ID="Hint" runat="server" TargetSelector=".dx-ellipsis" Animation="true" Position="top" 
                                ShowCallout ="false" TriggerAction="HoverAndFocus" />
                            <dx:ASPxGridView ID="dg" ClientInstanceName="dg" runat="server" KeyFieldName="ID;CompanyNo"
                                Width="100%" KeyboardSupport="false" Font-Names="Segoe UI" Font-Size="10pt"
                                OnRowValidating="dg_RowValidating" OnRowUpdating="dg_RowUpdating" 
                                OnContextMenuInitialize="dg_ContextMenuInitialize" OnContextMenuItemClick="dg_ContextMenuItemClick" 
                                OnCustomCallback="dg_CustomCallback" OnCustomButtonCallback="dg_CustomButtonCallback" >
                                
                                <settingsediting mode="Inline" />
                                <Settings GridLines="Horizontal" ShowFilterRow="False" ShowFilterRowMenu="True" ShowFooter="True" 
                                    ShowGroupPanel="False" ShowGroupFooter="VisibleAlways"
                                    ShowHorizontalScrollBar="True" ShowVerticalScrollBar="false" VerticalScrollableHeight="900" />
                                <SettingsBehavior AutoExpandAllGroups="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" 
                                    EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                                <ClientSideEvents EndCallback="function(s, e) { ASPxClientHint.Update(); }" /> <%--for ASPxHint--%>
                                <ClientSideEvents Init="OnInit" /> <%--Example: How to implement extended keyboard functionality in DevExpress--%>
                                <ClientSideEvents ContextMenuItemClick="OnContextMenuItemClick" /><%--View Menu Click from Context Menu Item--%>
                                <SettingsPager PageSize="15" />
                                <settingsdatasecurity allowdelete="False" allowinsert="False" />

                                <SettingsPopup>
                                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                                    <HeaderFilter Height="300" width="400">
                                        <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="768" MinHeight="300" />
                                    </HeaderFilter>
                                </SettingsPopup>

                                <SettingsSearchPanel Visible="false" />
                                <%--Settings-AllowSort="false" Settings-AllowAutoFilter="false" Settings-AllowGroup="false"--%>

                                <%--<settingstext searchpaneleditornulltext="Common Search" />--%>

                                <Columns>
                                    <dx:GridViewDataTextColumn readonly FieldName="CompanyName" Caption="Comp. Name" Width="7%" >
                                        <Settings AutoFilterCondition="Contains" />
                                        <cellstyle cssclass="text-center" wrap="true" />
                                        <editcellstyle wrap="true" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn readonly FieldName="GrpGrantParent" Caption="Zone" Width="8%">
                                        <Settings AutoFilterCondition="Contains" />
                                        <cellstyle cssclass="text-center" wrap="true" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn readonly FieldName="LedParent" Caption="Parent Gorup" Width="8%">
                                        <Settings AutoFilterCondition="Contains" />
                                        <cellstyle cssclass="text-center" wrap="true" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn readonly FieldName="LedName" Caption="Client Name" Width="11%" SortIndex="1" SortOrder="Ascending" >
                                        <Settings AutoFilterCondition="Contains" />
                                        <cellstyle cssclass="text-blue" wrap="true" />
                                        <%--<propertiestextedit maxlength="3500">
                                            <validationsettings setfocusonerror="True">
                                                <requiredfield isrequired="True" errortext="Required field !! Cant left blank !!"></requiredfield>
                                            </validationsettings>
                                        </propertiestextedit>--%>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn readonly FieldName="BillName" Caption="Bill No." Width="9%" >
                                        <Settings AutoFilterCondition="Contains" />
                                        <cellstyle cssclass="text-center" wrap="true" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataDateColumn readonly FieldName="Date" Caption="Date" Width="9%" SortIndex="0" SortOrder="Ascending">
                                        <PropertiesDateEdit DisplayFormatString="{0:dd-MM-yyyy}" EditFormatString="dd-MM-yyyy" />
                                        <Settings AutoFilterCondition="GreaterOrEqual" />
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataSpinEditColumn readonly FieldName="SalesAmt" Caption="Sales Amount" Width="12%" >
                                        <PropertiesSpinEdit DisplayFormatString="g" />
                                        <Settings AutoFilterCondition="GreaterOrEqual" />
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn readonly FieldName="DebitBal" Caption="Debit Balance" Width="12%" >
                                        <PropertiesSpinEdit DisplayFormatString="g" />
                                        <Settings AutoFilterCondition="GreaterOrEqual" />
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataTextColumn readonly FieldName="OverDueDays" Caption="Due Days" Width="6%" >
                                        <Settings AutoFilterCondition="Contains" />
                                        <cellstyle cssclass="text-center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataDateColumn FieldName="PaymentDate" Caption="Payment Date" Width="12%" >
                                        <cellstyle cssclass="text-center" wrap="true" />
                                        <PropertiesDateEdit DisplayFormatString="{0:dd-MM-yyyy}" EditFormatString="dd-MM-yyyy">
                                            <validationsettings setfocusonerror="True">
                                                <requiredfield isrequired="True" errortext="Required field !! Cant left blank !!"></requiredfield>
                                            </validationsettings>
                                        </PropertiesDateEdit>
                                        <Settings AutoFilterCondition="GreaterOrEqual" />
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Remark" Caption="Status" Width="13%" >
                                        <cellstyle cssclass="text-center" />
                                        <PropertiesComboBox IncrementalFilteringMode="StartsWith" DropDownStyle="DropDownList" 
                                            TextField="Remark" ValueField="Remark" ValueType="System.String" > 
                                            <validationsettings>
                                                <requiredfield isrequired="True" errortext="Required field !! Cant left blank !!"></requiredfield>
                                            </validationsettings>
                                            <%--<ClearButton DisplayMode="OnHover" />--%>        
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataMemoColumn FieldName="Remark1" Caption="Remark" Width="13%" >
                                        <propertiesmemoedit maxlength="4000" >
                                            <%--<validationsettings>
                                                <requiredfield isrequired="True" errortext="Required field !! Cant left blank !!"></requiredfield>
                                            </validationsettings>--%>
                                        </propertiesmemoedit>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataMemoColumn>
                                    <dx:GridViewDataTextColumn FieldName="ContactPerson" Caption="Contact Person" Width="7%" >
                                        <cellstyle cssclass="text-center" wrap="true" />
                                        <propertiestextedit maxlength="100" >
                                            <%--<validationsettings>
                                                <requiredfield isrequired="True" errortext="Required field !! Cant left blank !!"></requiredfield>
                                            </validationsettings>--%>
                                        </propertiestextedit>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>


                                    <dx:GridViewCommandColumn ShowClearFilterButton="true" ShowEditButton="true" ShowCancelButton="true" 
                                        ButtonRenderMode="Image" Caption=" " Width="6%" >
                                        <cellstyle wrap="false" />
                                    </dx:GridViewCommandColumn>
                                </Columns>

                                <Styles>
                                    <header wrap="True" horizontalalign="Center" verticalalign="Middle">
                                        <%--<paddings paddingleft="5px" paddingright="5px" />--%>
                                        <border borderstyle="None"></border>
                                    </header>
                                    <row HorizontalAlign="Left" VerticalAlign="Middle" /> <%--Wrap="false"--%>
                                    <AlternatingRow Enabled="false" />
                                    <filterrow backcolor="White" CssClass="p-t-10 p-b-10" />
                                    <cell>
                                        <paddings paddingbottom="15px" paddingleft="6px" paddingright="10px" paddingtop="15px" />
                                    </cell>
                                    <%--<footer backcolor="#BF4E6A" forecolor="White" />--%>
                                    <pagerbottompanel backcolor="White" />
                                    <filtercell>
                                        <border borderstyle="None"></border>
                                    </filtercell>
                                    <searchpanel backcolor="White" CssClass="searchpanel m-b-5" />
                                    <grouppanel backcolor="White" CssClass="searchpanel m-b-5" /> 
                                </Styles>
                                <SettingsCommandButton>
                                    <EditButton  RenderMode="Image">
                                        <Image ToolTip="Edit" Url="../Images/edit.png" width="20px" height="20px" />
                                    </EditButton>
                                    <UpdateButton  RenderMode="Image">
                                        <Image ToolTip="Update" Url="../Images/save.png" width="20px" height="20px" />
                                    </UpdateButton>
                                    <CancelButton  RenderMode="Image">
                                        <Image ToolTip="Cancel Changes" Url="../Images/cancel.png" width="20px" height="20px" />
                                    </CancelButton>
                                </SettingsCommandButton>
                                <SettingsExport EnableClientSideExportAPI="false" />
                                <SettingsContextMenu Enabled="true" EnableRowMenu="false">
                                    <RowMenuItemVisibility ExportMenu-Visible="false">
                                        <GroupSummaryMenu SummaryAverage="false" SummaryMax="false" SummaryMin="false" SummarySum="false" />
                                    </RowMenuItemVisibility>
                                </SettingsContextMenu>

                                <TotalSummary>
                                    <dx:ASPxSummaryItem FieldName="LedName" SummaryType="Count" DisplayFormat="Count : {0}" />
                                    <dx:ASPxSummaryItem FieldName="SalesAmt" SummaryType="Sum" DisplayFormat="₹ {0:N2}" />
                                    <dx:ASPxSummaryItem FieldName="DebitBal" SummaryType="Sum" DisplayFormat="₹ {0:N2}" />
                                </TotalSummary>

                                <GroupSummary>
                                    <%--<dx:ASPxSummaryItem FieldName="LedName" SummaryType="Count" DisplayFormat="Count : {0}" />--%>
                                    <dx:ASPxSummaryItem FieldName="SalesAmt" ShowInGroupFooterColumn="SalesAmt" SummaryType="Sum" DisplayFormat="₹ {0:N2}" />
                                    <dx:ASPxSummaryItem FieldName="DebitBal" ShowInGroupFooterColumn="DebitBal" SummaryType="Sum" DisplayFormat="₹ {0:N2}" />
                                </GroupSummary>

                                <Templates>
                                    <PagerBar>
                                        <br />
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton runat="server" ID="FirstButton" Text="First (T)" AccessKey="T" Enabled="<%# Container.Grid.PageIndex > 0 %>"
                                                        AutoPostBack="false" cssclass="dxb-outline">
                                                        <ClientSideEvents Click="function() { dg.GotoPage(0) }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton runat="server" ID="PrevButton" Text="Prev (P)" AccessKey="P" Enabled="<%# Container.Grid.PageIndex > 0 %>"
                                                        AutoPostBack="false" cssclass="dxb-outline">
                                                        <ClientSideEvents Click="function() { dg.PrevPage() }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox runat="server" ID="GotoBox" Width="30">
                                                        <ClientSideEvents Init="CustomPager.gotoBox_Init" ValueChanged="CustomPager.gotoBox_ValueChanged"
                                                            KeyPress="CustomPager.gotoBox_KeyPress" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <span style="white-space: nowrap">of
                                                                <%# Container.Grid.PageCount %>
                                                    </span>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton runat="server" ID="NextButton" Text="Next (N)" AccessKey="N" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>"
                                                        AutoPostBack="false" cssclass="dxb-outline">
                                                        <ClientSideEvents Click="function() { dg.NextPage() }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton runat="server" ID="LastButton" Text="Last (L)" AccessKey="L" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>"
                                                        AutoPostBack="false" cssclass="dxb-outline">
                                                        <ClientSideEvents Click="function() { dg.GotoPage(dg.GetPageCount() - 1); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td style="width: 55%"></td>
                                                <td style="white-space: nowrap">
                                                    <span style="white-space: nowrap">Records per page (R) : </span>
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox runat="server" ID="Combo" Width="60" DropDownWidth="50" ValueType="System.Int32"
                                                        OnLoad="PagerCombo_Load" AccessKey="R">
                                                        <Items>
                                                            <dx:ListEditItem Value="15" />
                                                            <dx:ListEditItem Value="25" />
                                                            <dx:ListEditItem Value="50" />
                                                            <dx:ListEditItem Value="100" />
                                                            <dx:ListEditItem Value="200" />
                                                        </Items>
                                                        <ClientSideEvents SelectedIndexChanged="CustomPager.combo_SelectedIndexChanged" />
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </PagerBar>
                                </Templates>

                                <border borderstyle="none" />

                            </dx:ASPxGridView>
                            <dx:ASPxGridViewExporter ID="dgExport" runat="server" GridViewID="dg" />
                        </div>
                        <asp:PlaceHolder ID="phpermission" runat="server" Visible="False">
                            <br /><br /><asp:Label ID="Label1" runat="server" Text="You dont have access to view this module !!" CssClass="redtext-bold"></asp:Label><br />
                            <asp:Label ID="Label2" runat="server" Text="Please contact your System Administrator !!" CssClass="redtext-bold"></asp:Label><br /><br /><br />
                        </asp:PlaceHolder>
                    </section>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

