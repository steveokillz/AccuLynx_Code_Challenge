<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="My_Questions.aspx.cs" Inherits="AccuLynx_Code_Challenge.My_Questions" EnableEventValidation="False" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="Main_Style_Sheet.css" />
    <title>Question</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="ErrorMsg" runat="server" Visible="false" ForeColor="RED"></asp:Label>
        </div>
            <div style="height: 54px"><asp:Label runat="server" Text="AccuLynx Code Challenge" CssClass="title"></asp:Label></div>
            <div style="width:100%; height: 74px;">
            <asp:Label ID="Label1" runat="server" CssClass="Username"></asp:Label>
                <asp:Menu ID="Menu1" CssClass="menuBar" runat="server" BackColor="#F7F6F3" Orientation="Horizontal" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="1em" ForeColor="#7C6F57" StaticSubMenuIndent="10px">
                    <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        
                        <asp:MenuItem NavigateUrl="~/Main.aspx" Text="Home" Value="Home"></asp:MenuItem>
                        
                        <asp:MenuItem Text="Help" Value="Help" NavigateUrl="~/Help.aspx"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
        <div>
            <asp:Label ID="QuestionTitle" runat="server" Text="Question: " Height="2em"></asp:Label>
            <asp:Label ID="Question" runat="server" Height="2em"></asp:Label>
            <asp:GridView ID="CommentGridfield" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"  autogeneratecolumns="false">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                <Columns>
                    <asp:BoundField HeaderText="Answer_ID" DataField="Answer_ID" Visible="false"/>
                    <asp:BoundField HeaderText="Answer" DataField="Question_Information" HtmlEncode="false" />  
                    <asp:BoundField HeaderText="Correct_Answer" DataField="Correct_Answer" Visible="false" />  
                    <asp:BoundField HeaderText="Score_Of_Answer" DataField="Score_Of_Answer" Visible="false" />  
                    <asp:BoundField HeaderText="button_disabled" DataField="button_disabled" Visible="false"/>  
                    <asp:TemplateField HeaderText="Submit Answer">
                        <ItemTemplate>
                            <asp:Button ID="submitBtn" runat="server" CommandName="CheckAnswer" Text="CheckAnswer" OnClick="submitBtn_Click" Enabled='<%# Eval("button_disabled").Equals("False") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                    </div>
        <div>
            <asp:Label ID="QuestionID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UserID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="AnswerID" runat="server" Text="Label" Visible="false"></asp:Label>
            <asp:Label ID="TotalScore" runat="server" Text="Label" Visible="false"></asp:Label>
            <asp:Label ID="IsCorrect" runat="server" Text="Label" Visible="false"></asp:Label>

        </div>
    </form>
</body>
</html>
