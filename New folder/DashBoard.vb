Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Xml
Imports ClassLibrary
Imports Microsoft.VisualBasic
Imports Ssetu.SS

Public Class DashBoard
    Dim objDal As New DAL()

    'Public Function CM_GetOutstandingTotal(ByVal CompanyName As String, ByVal Zone As String,
    '                                        ByVal LedParent As String, ByVal LedName As String,
    '                                        ByVal Login_Code As Integer, ByVal SecLevel_Code As Integer) As DataSet
    '    Dim ds As New DataSet
    '    Dim sqlpara() As SqlParameter = {objDal.MakeInParams("@CompanyName", SqlDbType.NVarChar, 0, CompanyName),
    '                                     objDal.MakeInParams("@Zone", SqlDbType.NVarChar, 0, Zone),
    '                                     objDal.MakeInParams("@LedParent", SqlDbType.NVarChar, 0, LedParent),
    '                                     objDal.MakeInParams("@LedName", SqlDbType.NVarChar, 0, LedName),
    '                                     objDal.MakeInParams("@Login_Code", SqlDbType.Int, 0, Login_Code),
    '                                     objDal.MakeInParams("@SecLevel_Code", SqlDbType.Int, 0, SecLevel_Code)}
    '    objDal.RunProc("[dbo].[CM_GetOutstandingTotal]", sqlpara, ds) '' "CM_GetOutstandingTotal" -- Calling a Stored Procedure
    '    Return ds
    'End Function
    Public Function CM_GetOutstandingTotal(ByVal CompanyName As String, ByVal Zone As String,
                                           ByVal LedParent As String, ByVal LedName As String,
                                           ByVal Login_Code As Integer, ByVal SecLevel_Code As Integer) As DataSet
        Dim ds As New DataSet
        Try

            Dim conStr As String = ConfigurationManager.ConnectionStrings("MSConnectionString").ConnectionString
            Using con As New SqlConnection(conStr)
                Using cmd As New SqlCommand("[dbo].[CM_GetOutstandingTotal]", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@CompanyName", CompanyName)
                    cmd.Parameters.AddWithValue("@Zone", Zone)
                    cmd.Parameters.AddWithValue("@LedParent", LedParent)
                    cmd.Parameters.AddWithValue("@LedName", LedName)
                    cmd.Parameters.AddWithValue("@Login_Code", Login_Code)
                    cmd.Parameters.AddWithValue("@SecLevel_Code", SecLevel_Code)

                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error executing [dbo].[CM_GetOutstandingTotal]: " & ex.Message, ex)
        End Try
        Return ds
    End Function

    Public Function CM_GetOutstandingData(ByVal CompanyName As String, ByVal Zone As String,
                                           ByVal LedParent As String, ByVal LedName As String,
                                           ByVal Login_Code As Integer, ByVal SecLevel_Code As Integer) As DataSet
        Dim ds As New DataSet
        Try
            ' Get connection string from Web.config
            Dim conStr As String = ConfigurationManager.ConnectionStrings("MSConnectionString").ConnectionString

            Using con As New SqlConnection(conStr)
                Using cmd As New SqlCommand("[dbo].[CM_GetOutstandingData]", con)
                    cmd.CommandType = CommandType.StoredProcedure

                    ' ✅ Add all parameters exactly as defined in SQL
                    cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 0).Value = CompanyName
                    cmd.Parameters.Add("@LedParent", SqlDbType.NVarChar, 0).Value = LedParent
                    cmd.Parameters.Add("@LedName", SqlDbType.NVarChar, 0).Value = LedName

                    ' ✅ Fill dataset
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error executing [dbo].[CM_GetOutstandingData]: " & ex.Message, ex)
        End Try
        'Dim sqlpara() As SqlParameter = {objDal.MakeInParams("@CompanyName", SqlDbType.NVarChar, 0, CompanyName),
        '                                 objDal.MakeInParams("@Zone", SqlDbType.NVarChar, 0, Zone),
        '                                 objDal.MakeInParams("@LedParent", SqlDbType.NVarChar, 0, LedParent),
        '                                 objDal.MakeInParams("@LedName", SqlDbType.NVarChar, 0, LedName),
        '                                 objDal.MakeInParams("@Login_Code", SqlDbType.Int, 0, Login_Code),
        '                                 objDal.MakeInParams("@SecLevel_Code", SqlDbType.Int, 0, SecLevel_Code)}
        'objDal.RunProc("CM_GetOutstandingData", sqlpara, ds) '' "CM_GetOutstandingData" -- Calling a Stored Procedure
        Return ds
    End Function

    'Public Function CM_GetOSUniqueRecord(ByVal ColName As String, ByVal AllowBlank As Integer,
    '                                     ByVal Login_Code As Integer, ByVal SecLevel_Code As Integer) As DataSet
    '    Dim ds As New DataSet
    '    Dim sqlpara() As SqlParameter = {objDal.MakeInParams("@ColName", SqlDbType.NVarChar, 0, ColName),
    '                                     objDal.MakeInParams("@AllowBlank", SqlDbType.Int, 0, AllowBlank),
    '                                     objDal.MakeInParams("@Login_Code", SqlDbType.Int, 0, Login_Code),
    '                                     objDal.MakeInParams("@SecLevel_Code", SqlDbType.Int, 0, SecLevel_Code)}
    '    objDal.RunProc("CM_GetOSUniqueRecord", sqlpara, ds) '' "CM_GetOSUniqueRecord" -- Calling a Stored Procedure
    '    Return ds
    'End Function
    Public Function CM_GetOSUniqueRecord(ByVal ColName As String,
                                     ByVal AllowBlank As Integer,
                                     ByVal Login_Code As Integer,
                                     ByVal SecLevel_Code As Integer) As DataSet

        Dim ds As New DataSet

        Try
            ' Get connection string from Web.config
            Dim conStr As String = ConfigurationManager.ConnectionStrings("MSConnectionString").ConnectionString

            Using con As New SqlConnection(conStr)
                Using cmd As New SqlCommand("[dbo].[CM_GetOSUniqueRecord]", con)
                    cmd.CommandType = CommandType.StoredProcedure

                    '' ✅ Add all parameters exactly as defined in SQL
                    'cmd.Parameters.AddWithValue("@ColName", ColName)
                    'cmd.Parameters.AddWithValue("@AllowBlank", AllowBlank)
                    'cmd.Parameters.AddWithValue("@Login_Code", Login_Code)
                    'cmd.Parameters.AddWithValue("@SecLevel_Code", SecLevel_Code)
                    ' ✅ Add all parameters exactly as defined in SQL
                    cmd.Parameters.Add("@ColName", SqlDbType.NVarChar, 0).Value = ColName
                    cmd.Parameters.Add("@AllowBlank", SqlDbType.Int, 0).Value = AllowBlank
                    cmd.Parameters.Add("@Login_Code", SqlDbType.Int, 0).Value = Login_Code
                    cmd.Parameters.Add("@SecLevel_Code", SqlDbType.Int, 0).Value = SecLevel_Code

                    ' ✅ Fill dataset
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error executing [dbo].[CM_GetOSUniqueRecord]: " & ex.Message, ex)
        End Try

        Return ds
    End Function
    Public Function CM_GetRemarkDetails() As DataSet
        Dim ds As New DataSet
        Dim conStr As String = ConfigurationManager.ConnectionStrings("MSConnectionString").ConnectionString

        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("[dbo].[CM_GetRemarkDetails]", con)
                cmd.CommandType = CommandType.StoredProcedure
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds)
            End Using
        End Using
        'objDal.RunProc("CM_GetRemarkDetails", ds) '' "CM_GetRemarkDetails" -- Calling a Stored Procedure
        Return ds
    End Function

    Public Function CM_UpdateRecord(ByVal ID As Integer, ByVal CompanyNo As String, CompanyName As String,
                                    ByVal LedName As String, ByVal BillName As String, ByVal PaymentDate As DateTime,
                                    ByVal Remark As String, ByVal Remark1 As String, ByVal ContactPerson As String,
                                ByVal Login_Code As Integer, ByVal AlterDate As DateTime) As DataSet
        Dim ds As New DataSet

        Try
            Dim conStr As String = ConfigurationManager.ConnectionStrings("MSConnectionString").ConnectionString
            Using con As New SqlConnection(conStr)
                Using cmd As New SqlCommand("[dbo].[CM_UpdateRecord]", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@ID", ID)
                    cmd.Parameters.AddWithValue("@CompanyNo", CompanyNo)
                    cmd.Parameters.AddWithValue("@CompanyName", CompanyName)
                    cmd.Parameters.AddWithValue("@LedName", LedName)
                    cmd.Parameters.AddWithValue("@BillName", BillName)
                    cmd.Parameters.AddWithValue("@PaymentDate", PaymentDate)
                    cmd.Parameters.AddWithValue("@Remark", Remark)
                    cmd.Parameters.AddWithValue("@Remark1", Remark1)
                    cmd.Parameters.AddWithValue("@ContactPerson", ContactPerson)
                    cmd.Parameters.AddWithValue("@Login_Code", Login_Code)
                    cmd.Parameters.AddWithValue("@AlterDate", AlterDate)

                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error executing [dbo].[CM_UpdateRecord]: " & ex.Message, ex)
        End Try


        'Dim sqlpara() As SqlParameter = {objDal.MakeInParams("@ID", SqlDbType.Int, 0, ID),
        '                                objDal.MakeInParams("@CompanyNo", SqlDbType.NVarChar, 0, CompanyNo),
        '                                objDal.MakeInParams("@CompanyName", SqlDbType.NVarChar, 0, CompanyName),
        '                                objDal.MakeInParams("@LedName", SqlDbType.NVarChar, 0, LedName),
        '                                objDal.MakeInParams("@BillName", SqlDbType.NVarChar, 0, BillName),
        '                                objDal.MakeInParams("@PaymentDate", SqlDbType.Date, 0, PaymentDate),
        '                                objDal.MakeInParams("@Remark", SqlDbType.NVarChar, 0, Remark),
        '                                objDal.MakeInParams("@Remark1", SqlDbType.NVarChar, 0, Remark1),
        '                                objDal.MakeInParams("@ContactPerson", SqlDbType.NVarChar, 0, ContactPerson),
        '                                objDal.MakeInParams("@Login_Code", SqlDbType.Int, 0, Login_Code),
        '                                objDal.MakeInParams("@AlterDate", SqlDbType.DateTime, 0, AlterDate)}
        'objDal.RunProc("CM_UpdateRecord", sqlpara, ds) '' "CM_UpdateRecord" -- Calling a Stored Procedure
        Return ds
    End Function

    Public Function CM_GetEditLogDetails() As DataSet
        Dim ds As New DataSet
        Dim conStr As String = ConfigurationManager.ConnectionStrings("MSConnectionString").ConnectionString

        Using con As New SqlConnection(conStr)
            Using cmd As New SqlCommand("[dbo].[CM_GetEditLogDetails]", con)
                cmd.CommandType = CommandType.StoredProcedure
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds)
            End Using
        End Using
        'objDal.RunProc("CM_GetEditLogDetails", ds) '' "CM_GetEditLogDetails" -- Calling a Stored Procedure
        Return ds
    End Function
End Class
