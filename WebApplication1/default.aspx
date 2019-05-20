<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WebFormAndWebApiLab._default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span>Current date & time:<%=DateTime.Now%></span><br /><br />
        <h1><asp:Label ID="lbl" runat="server" Text="" Font-Bold="true"></asp:Label></h1>
        <hr />
        <a href="/api/demo/get/1" >Get Id=1 (No login required)</a><br /><br />
        <a href="/api/demo/get" >Get all (Requires you to be logged in)</a><br /><br />
        <a href="/api/demo/gettotalsales" >Get total sales (Requires to be logged in and be an administrator)</a><br /><br />
        <hr />
        User Id:<br />
        <asp:TextBox id="userId" runat="server" text="user1" ToolTip="Enter user1"></asp:TextBox>&nbsp;Enter user1 as user id.<br />
        Password:<br />
        <asp:TextBox id="pwd" runat="server" ToolTip="Enter Welcome123@" TextMode="Password"></asp:TextBox>&nbsp;Enter "Welcome123@" as password.<br />
        <asp:Button runat="server" ID="loginBtn" Text="Login" OnClick="loginBtn_Click" />
        <hr />
        <asp:Button runat="server" ID="supervisorBtn" Text="Become administrator" OnClick="becomeAdmin_Click" /><br /><br />
        <asp:Button runat="server" ID="normalUserBtn" Text="Become normal user" OnClick="becomeNormalUser_Click" /><br /><br />
        <asp:Button runat="server" ID="logoffBtn" Text="Logout" OnClick="logoffBtn_Click" /><br /><br />
    </div>
    </form>
</body>
</html>
