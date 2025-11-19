Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports ClassLibrary
Imports System.IO
Imports Ssetu.SS.Init
Imports SsetuSoft.Web
Imports System.Net.Mail
Imports DevExpress.Web
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Utils
Imports System.Drawing
Imports DevExpress.Office.Utils.HdcOriginModifier
Imports DevExpress.Web.Data
Imports System.Xml.Serialization
Imports System.Activities.Expressions
Imports DevExpress.Web.Internal.XmlProcessor
Imports System.Threading
Imports DevExpress.Utils.About
Imports System.Windows.Forms
Imports DevExpress.XtraRichEdit.Commands
Imports DevExpress.Web.Rendering
Imports DevExpress.XtraExport.Helpers
Imports DevExpress.Web.Internal
Imports DevExpress.Data.Helpers
Imports ASP

Partial Class spadmin_outstanding
    Inherits System.Web.UI.Page
    Dim ds_Login As New DataSet
    Dim objCommon As New Common
    Dim objDashBoard As New DashBoard

    Private Login_Code As Integer, SecLevel_Code As Integer
    Private Const PageSizeSessionKey As String = "os-pager"
    Private isReadOnlyMode As Boolean = False

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        dg.SettingsPager.PageSize = GridPageSize
    End Sub
    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        dgExport.FileName = "Outstanding"
        dgExport.Styles.Default.Font.Name = "Calibri"
        dgExport.Styles.Default.Font.Size = "11"
        dgExport.Styles.Header.BackColor = dg.Styles.Header.BackColor
        dgExport.Styles.Header.ForeColor = dg.Styles.Header.ForeColor
        dgExport.Styles.Header.Font.Bold = True
        dgExport.Styles.Footer.BackColor = dg.Styles.Footer.BackColor
        dgExport.Styles.Footer.ForeColor = dg.Styles.Footer.ForeColor
        dgExport.Styles.Footer.Font.Bold = True
    End Sub

    'Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    '    Dim reports As HtmlGenericControl
    '    reports = Page.Master.FindControl("reports")
    '    reports.Attributes.Add("class", "collapse in")

    '    If Session("SPlogin") Is Nothing Then
    '        Response.Redirect(SiteCommon.RedirectSPDefaultURL)
    '        Return
    '    End If

    '    ds_Login = Session("SPlogin")
    '        SecLevel_Code = Convert.ToInt32(ds_Login.Tables("SPlogin").Rows(0).Item("SecLevel_Code"))
    '        Login_Code = Convert.ToInt32(ds_Login.Tables("SPlogin").Rows(0).Item("SPLogin_Code"))

    '    If Not IsPostBack Then
    '        dg.SearchPanelFilter = ""
    '        dg.FilterExpression = ""
    '        Display()   ' <-- RUNS ONLY ON FIRST LOAD
    '    End If


    'End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' Expand menu on master page
        Dim reports As HtmlGenericControl = Page.Master.FindControl("reports")
        reports.Attributes.Add("class", "collapse in")

        ' Session check for logout
        If Session("SPlogin") Is Nothing Then
            Response.Redirect(SiteCommon.RedirectSPDefaultURL)
            Return  ' **Important: stop further execution**
        End If

        ' Read session only once
        ds_Login = Session("SPlogin")
        SecLevel_Code = Convert.ToInt32(ds_Login.Tables("SPlogin").Rows(0).Item("SecLevel_Code"))
        Login_Code = Convert.ToInt32(ds_Login.Tables("SPlogin").Rows(0).Item("SPLogin_Code"))

        ' Bind grid ONLY on first load
        If Not IsPostBack Then
            dg.SearchPanelFilter = ""
            dg.FilterExpression = ""

            ' <-- Correct: bind ONLY once
        End If
        Display()
    End Sub


    Private Sub Display()
        Dim ds As New DataSet
        Dim CompanyName As String, Zone As String, LedParent As String, LedName As String

        CompanyName = "ALL COMPANY"
        Zone = "ALL ZONE"
        LedParent = "ALL PARENT GROUPS"
        LedName = "ALL DEBTORS"

        ds = objDashBoard.CM_GetOutstandingData(CompanyName, Zone, LedParent, LedName, Login_Code, SecLevel_Code)
        dg.DataSource = ds

        '' BIND FILTER OPTIONS IN DATAGRIDEVIEW
        ds = objDashBoard.CM_GetRemarkDetails()
        CType(dg.Columns(CStr("Remark")), GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = ds

        dg.DataBind()
        dg.ExpandAll()
    End Sub

    Protected Sub dg_CustomButtonCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomButtonCallbackEventArgs)
        isReadOnlyMode = True

        If e.ButtonID = "btnView" Then
            dg.StartEdit(e.VisibleIndex)
            For Each column As GridViewDataColumn In dg.DataColumns
                column.ReadOnly = True
            Next
        End If
    End Sub

    Private Sub dg_CellEditorInitialize(sender As Object, e As ASPxGridViewEditorEventArgs) Handles dg.CellEditorInitialize
        'If e.Column.FieldName = "ActAmtRecd" Then
        '    Dim ActAmtRecd As ASPxSpinEdit = CType(e.Editor, ASPxSpinEdit)
        '    ActAmtRecd.Text = 0
        'End If

        If e.Column.FieldName = "Remark" Then
            Dim Remark As ASPxComboBox = CType(e.Editor, ASPxComboBox)
            Remark.DataBind()
        End If
    End Sub

    'Protected Sub dg_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles dg.CustomCallback
    '    Dim parakey() As String = e.Parameters.Split("|"c)

    '    If parakey(1) = "FeildGrp" Then
    '        ApplyLayout(Int32.Parse(parakey(0)))
    '    ElseIf parakey(1) = "PagerCombo" Then
    '        GridPageSize = Integer.Parse(parakey(0))
    '        dg.SettingsPager.PageSize = GridPageSize
    '        dg.DataBind()
    '    End If
    'End Sub

    Protected Sub dg_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles dg.CustomCallback
        Dim parakey() As String = If(String.IsNullOrEmpty(e.Parameters), New String() {}, e.Parameters.Split("|"c))

        If parakey.Length = 0 Then
            Return
        End If

        ' e.g. expected "15|PagerCombo" or "1|FeildGrp"
        If parakey.Length >= 2 Then
            Dim key = parakey(0)
            Dim cmd = parakey(1)

            If cmd = "FeildGrp" Then
                Dim idx As Integer
                If Integer.TryParse(key, idx) Then
                    ApplyLayout(idx)
                    dg.ExpandAll()       ' ensure groups expanded after layout
                End If
            ElseIf cmd = "PagerCombo" Then
                Dim newSize As Integer
                If Integer.TryParse(key, newSize) Then
                    GridPageSize = newSize
                    dg.SettingsPager.PageSize = GridPageSize

                    ' Rebind properly
                    Display()
                End If
            End If
        End If
    End Sub

    Protected Sub dg_StartRowEditing(sender As Object, e As ASPxStartRowEditingEventArgs) Handles dg.StartRowEditing
        UpdateEditFormFields(1)
    End Sub

    Private Sub dg_CancelRowEditing(sender As Object, e As ASPxStartRowEditingEventArgs) Handles dg.CancelRowEditing
        UpdateEditFormFields(0)
    End Sub

    Private Sub UpdateEditFormFields(ByVal CalledFrom As Byte)
        For Each column As GridViewDataColumn In dg.DataColumns
            If column.ReadOnly = True And column.FieldName <> "LedName" And column.FieldName <> "BillName" Then
                If CalledFrom = 0 Then column.Visible = True '' Update / Cancel Button Click
                'If CalledFrom = 1 Then column.Visible = isReadOnlyMode '' Edit Button Click
            Else

            End If
        Next
    End Sub

    Private Sub dg_CommandButtonInitialize(sender As Object, e As ASPxGridViewCommandButtonEventArgs) Handles dg.CommandButtonInitialize
        If isReadOnlyMode = True Then
            dg.SettingsText.PopupEditFormCaption = ":: View Form ::"
            If e.ButtonType = DevExpress.Web.ColumnCommandButtonType.Update Or e.ButtonType = DevExpress.Web.ColumnCommandButtonType.Cancel Then
                e.Visible = False
            End If
        End If
    End Sub

    Protected Sub dg_ContextMenuInitialize(sender As Object, e As ASPxGridViewContextMenuInitializeEventArgs)
        'If e.MenuType = GridViewContextMenuType.Rows Then
        '    e.ContextMenu.Items.Add("View", "View").Image.IconID = "dashboards_zoom_svg_16x16"
        '    ''e.ContextMenu.Items.Add("View", "View").Image.Url = "../images/view.png"
        'End If
    End Sub

    Private Sub dg_FillContextMenuItems(sender As Object, e As ASPxGridViewContextMenuEventArgs) Handles dg.FillContextMenuItems
        If e.MenuType = GridViewContextMenuType.Rows Then
            Dim item = e.CreateItem("View", "View")
            item.Image.IconID = "zoom_zoom_16x16"
            item.BeginGroup = True
            e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.DeleteRow), item)
        End If
    End Sub

    Protected Sub dg_ContextMenuItemClick(ByVal sender As Object, ByVal e As ASPxGridViewContextMenuItemClickEventArgs)
        If e.Item.Name = "View" Then
            isReadOnlyMode = True

            dg.StartEdit(e.ElementIndex)
            For Each column As GridViewDataColumn In dg.DataColumns
                column.ReadOnly = True
            Next
        End If
    End Sub

    Sub dg_RowValidating(sender As Object, e As ASPxDataValidationEventArgs)
        For Each column As GridViewDataColumn In dg.DataColumns
            If column.ReadOnly = False Then
                Dim TextBoxColumn As GridViewDataTextColumn = TryCast(column, GridViewDataTextColumn)
                Dim ComboBoxColumn As GridViewDataComboBoxColumn = TryCast(column, GridViewDataComboBoxColumn)
                Dim DateColumn As GridViewDataDateColumn = TryCast(column, GridViewDataDateColumn)

                If column.GetType = GetType(GridViewDataTextColumn) Then
                    If TextBoxColumn IsNot Nothing Then
                        If TextBoxColumn.PropertiesTextEdit.ValidationSettings.RequiredField.IsRequired Then
                            If e.NewValues(column.FieldName) Is Nothing Then
                                e.Errors(column) = column.Caption & " can't left blank!"
                            End If
                        Else
                            If Not e.NewValues(column.FieldName) Is Nothing AndAlso e.NewValues(column.FieldName).ToString().Length > TextBoxColumn.PropertiesTextEdit.MaxLength Then
                                e.Errors(column) = "Data out of Max Length!"
                            End If
                        End If
                    End If
                ElseIf column.GetType = GetType(GridViewDataComboBoxColumn) Then
                    If ComboBoxColumn IsNot Nothing AndAlso ComboBoxColumn.PropertiesComboBox.ValidationSettings.RequiredField.IsRequired Then
                        If e.NewValues(column.FieldName) Is Nothing Then
                            e.Errors(column) = column.Caption & " can't left blank!"
                        End If
                    End If
                ElseIf column.GetType = GetType(GridViewDataDateColumn) Then
                    If e.NewValues(column.FieldName) Is Nothing Then
                        e.Errors(column) = column.Caption & " can't left blank!"
                    ElseIf IsDate(e.NewValues(column.FieldName).ToString()) = False Then
                        e.Errors(column) = column.Caption & " must be in proper format!"
                    End If
                End If

                'If column.FieldName = "ActAmtRecd" Then
                '    If e.NewValues(column.FieldName) < 0 Then
                '        e.Errors(column) = column.Caption & " cant accept negative values!"
                '    End If
                'End If
            End If
        Next column
    End Sub

    Sub dg_RowUpdating(sender As Object, e As ASPxDataUpdatingEventArgs)
        Dim ds As New DataSet
        Dim ID As Integer
        Dim CompanyNo As String, CompanyName As String, LedName As String, BillName As String
        Dim Remark As String, Remark1 As String, ContactPerson As String
        Dim PaymentDate As DateTime

        ID = e.Keys("ID")
        CompanyNo = e.Keys("CompanyNo")
        CompanyName = e.OldValues("CompanyName") & ""
        LedName = e.OldValues("LedName") & ""
        BillName = e.OldValues("BillName") & ""
        PaymentDate = e.NewValues("PaymentDate")
        Remark = e.NewValues("Remark") & ""
        Remark1 = e.NewValues("Remark1") & ""
        ContactPerson = e.NewValues("ContactPerson") & ""

        If ID > 0 Then
            ds = objDashBoard.CM_UpdateRecord(ID, CompanyNo, CompanyName, LedName, BillName, PaymentDate, Remark, Remark1, ContactPerson,
                                              Login_Code, DateTime.Now)

            e.Cancel = True
            dg.CancelEdit()

            Display() ''Re-Bind Grid with updated values
            UpdateEditFormFields(0)
        End If
    End Sub

    Private Sub dg_RowUpdated(sender As Object, e As ASPxDataUpdatedEventArgs) Handles dg.RowUpdated
        MsgBox("Updated")
    End Sub

    Private Sub ApplyLayout(ByVal layoutIndex As Integer)
        dg.BeginUpdate()
        Try
            dg.ClearSort()
            Select Case layoutIndex
                Case 1
                    dg.GroupBy(dg.Columns("LedName"))
            End Select
        Finally
            dg.EndUpdate()
        End Try
        dg.ExpandAll()
    End Sub

    Protected Property GridPageSize() As Integer
        Get
            If Session(PageSizeSessionKey) Is Nothing Then
                Return dg.SettingsPager.PageSize
            End If
            Return CInt(Fix(Session(PageSizeSessionKey)))
        End Get
        Set(ByVal value As Integer)
            Session(PageSizeSessionKey) = value
        End Set
    End Property

    Protected Sub dg_CustomErrorText(sender As Object, e As ASPxGridViewCustomErrorTextEventArgs) Handles dg.CustomErrorText

        If TypeOf e.Exception Is SqlClient.SqlException Then
            e.ErrorText = e.Exception.Message
        Else
            e.ErrorText = "Internal error occured !! Please try after some time !!"
        End If
    End Sub

    Protected Sub dg_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles dg.HtmlDataCellPrepared
        If e.DataColumn.FieldName = "SalesAmt" Or e.DataColumn.FieldName = "DebitBal" Then
            ' Get the value of the cell (PendingAmt)
            Dim Amount As Double = Convert.ToDouble(e.CellValue)

            ' Format the PendingAmt using the Indian numbering system
            Dim FormattedAmount As String = objCommon.FormatToIndianCurrency(Amount)

            ' Set the formatted value to the cell
            e.Cell.Text = "₹ " + FormattedAmount
        End If
    End Sub

    Protected Sub dg_SummaryDisplayText(sender As Object, e As ASPxGridViewSummaryDisplayTextEventArgs) Handles dg.SummaryDisplayText
        If e.Item.FieldName = "SalesAmt" Or e.Item.FieldName = "DebitBal" Then
            ' Get the value of the summary (total for PendingAmt)
            Dim TotalAmount As Double = Convert.ToDouble(Val(e.Value))

            ' Format the total PendingAmt using the Indian numbering system
            Dim FormattedAmount As String = objCommon.FormatToIndianCurrency(TotalAmount)

            ' Set the formatted value to the summary
            e.Text = "₹ " + FormattedAmount
        End If
    End Sub

    Protected Sub btnXlsxExport_Click(ByVal sender As Object, ByVal e As EventArgs)
        dgExport.WriteXlsxToResponse()
    End Sub

    Protected Sub PagerCombo_Load(ByVal sender As Object, ByVal e As EventArgs)
        TryCast(sender, ASPxComboBox).Value = dg.SettingsPager.PageSize
    End Sub

    Protected Sub btnShowGroupPanel_Click(sender As Object, e As EventArgs)
        ApplyLayout(0)
        dg.Settings.ShowGroupPanel = Not dg.Settings.ShowGroupPanel
    End Sub

    Protected Sub btnShowSearchPanel_Click(sender As Object, e As EventArgs)
        dg.SearchPanelFilter = ""
        dg.SettingsSearchPanel.Visible = Not dg.SettingsSearchPanel.Visible
    End Sub

    Protected Sub btnShowFilter_Click(sender As Object, e As EventArgs)
        dg.FilterExpression() = ""
        dg.Settings.ShowFilterRow = Not dg.Settings.ShowFilterRow
        dg.Settings.ShowHeaderFilterButton = Not dg.Settings.ShowHeaderFilterButton

        For Each column As GridViewDataColumn In dg.DataColumns
            If column.GetType = GetType(GridViewDataDateColumn) Then
                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.DateRangePicker
            Else
                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList
            End If
        Next column

    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/spadmin/outstanding.aspx")
    End Sub
End Class
