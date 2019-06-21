<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="AccuLynx_Code_Challenge.Help" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Help</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>Setup</div>
            <div>1. Alter the webconfig command as well as the connection string in the cs files</div>
            <div>2. Run the sql code thats in the SqlCode.Txt</div>
            <div style="height: 79px">3. Start the page as MAIN</div>

           <div> How to play this game</div>
            
            <div>1. On the main page select a questions off the list</div>
            <div>2. The questions will show up along with a series of answers</div>
           <div> 3. Pick the best one to win the question.</div>
           <div> 4. The scoring is based off of how many guesses it takes for you to answer the questions</div>
           <div> For example, if it takes you one guess then it would be 10 pts, the max number of guesses is 5</div>
           <div> 5.The Hall of fame grabs the top 5 users from the database and displays them along with their current points</div>

            <div>NOT IMPLIMENTED:</div>
            <div>I did not get a chance for a different user to select the same question that someone has already answered.</div>

            <div>GOOD LUCK AND HAVE FUN!!!!</div>
        </div>
    </form>
</body>
</html>
