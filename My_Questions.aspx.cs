using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.UI.WebControls;

namespace AccuLynx_Code_Challenge
{
    public partial class My_Questions : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

                string userMachine = HttpContext.Current.Server.MachineName;
                Label1.Text = "Welcome User: " + userMachine;
                QuestionID.Text = Request["ID"].ToString();
                getUserID(userMachine);
                check_Questions();
                fillGridview();
                updateQuestionList();
                getQuestion();


        }
        //Help limit API calls
        protected void check_Questions()
        {
            string connection = connURL();
            string query = "Select TOP(1) Question_ID from dbo.Questions where Question_ID = @Question_ID and User_ID = @User_ID";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@Question_ID", QuestionID.Text.ToString());
            command.Parameters.AddWithValue("@User_ID", UserID.Text.ToString());
            conn.Open();
            using (SqlDataReader read = command.ExecuteReader())
            {
                //This is to check if there is an actual value in here!
                if (read.HasRows.Equals(false))
                {
                    GetStackQuestions();
                }
            }
            conn.Close();
        }
        //This willuse JSON to get the comments and the correct answer to the question
        //We can either pull all the comments with the correct question
        //Or we can pull the correct comment with random comments, Either Or should work.
        protected void GetStackQuestions()
        {
            var URL = "https://api.stackexchange.com/2.2/questions/" + QuestionID.Text.ToString() + "/answers?&site=stackoverflow&filter=withbody";
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(URL).Result;
            if (response.IsSuccessStatusCode)
            {
                string JSON = response.Content.ReadAsStringAsync().Result;

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(JSON);

                string connection = connURL();
                string query = "If not exists(Select 1 from Questions where Answer_ID = @Answer_ID) Insert into dbo.Questions Values (@Question_ID, @Answer_ID, @Question_Information, @User_ID, @Correct_Answer, @Score_Of_Answer, @Button_disabled)";
                //Set up the connection string
                SqlConnection conn = new SqlConnection(connection);
                try
                {

                    foreach (Item stacklist in obj.Items)
                    {
                        SqlCommand command = new SqlCommand(query, conn);
                        command.Parameters.AddWithValue("@Question_ID", QuestionID.Text.ToString());
                        command.Parameters.AddWithValue("@Answer_ID", stacklist.answer_id);
                        command.Parameters.AddWithValue("@Question_Information", stacklist.Body);
                        command.Parameters.AddWithValue("@User_ID", UserID.Text.ToString());
                        command.Parameters.AddWithValue("@Correct_Answer", stacklist.is_accepted);
                        command.Parameters.AddWithValue("@Score_Of_Answer", stacklist.Score);
                        command.Parameters.AddWithValue("@Button_disabled", 0);
                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                catch (Exception e)
                {
                    ErrorMsg.Visible = true;
                    ErrorMsg.Text = "There was an error getting the getting the comments for this question. please diagnose this error: " + e.ToString();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //now we will populate the gridview with the selected answers
        protected void fillGridview()
        {
            //Call the SQL method to get questions

            string connection = connURL();
            //Since this is a 1 time call then we can use a sql query
            //TODO make this a stored procedure for further protection against sql injection if necessary

            string query = "Select Answer_ID, Question_Information, Correct_Answer, Score_Of_Answer, button_disabled from Questions where Question_ID = @Question_ID ";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@Question_ID", QuestionID.Text);
            conn.Open();
            //Databind the data into the sql statements
            SqlDataReader read = command.ExecuteReader();
            CommentGridfield.DataSource = read;
            CommentGridfield.DataBind();
            conn.Close();
        }

        //New owner of the questions needs to be updated in the table
        protected void updateQuestionList()
        {
            string connection = connURL();

            string query = "Update Question_List set Current_Owner = @Owner where Question_ID = @Question";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                //Update the Questions_List table to give them the owner id
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Owner", UserID.Text.ToString());
                command.Parameters.AddWithValue("@Question", QuestionID.Text.ToString());
                conn.Open();
                command.ExecuteNonQuery();
                //Close the connection
                conn.Close();
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error updating the owner for the question: " + e.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        public partial class RootObject
        {
            public Item[] Items { get; set; }
            public bool HasMore { get; set; }
            public long QuotaMax { get; set; }
            public long QuotaRemaining { get; set; }
        }

        public partial class Item
        {
            public Owner Owner { get; set; }
            public bool is_accepted { get; set; }
            public long Score { get; set; }
            public long LastActivityDate { get; set; }
            public long? LastEditDate { get; set; }
            public long CreationDate { get; set; }
            public long answer_id { get; set; }
            public long QuestionId { get; set; }
            public string Body { get; set; }
        }

        public partial class Owner
        {
            public long Reputation { get; set; }
            public long UserId { get; set; }
            public string UserType { get; set; }
            public Uri ProfileImage { get; set; }
            public string DisplayName { get; set; }
            public Uri Link { get; set; }
        }

        private static string connectionString()
        {
            //NOTE: YOU WILL HAVE TO CHANGE THIS CONNECTION STRING SINCE IT WILL BE DIFFERENT WHEN YOU USE IT!!
            string connectionurl = "Data Source=DESKTOP-NOTS8IG;Initial Catalog=Acculynx;Integrated Security=True";
            return connectionurl;
        }

        public string connURL()
        {
            string UrlString = connectionString();
            return UrlString;
        }

        //get the userid
        protected void getUserID(string user)
        {
            string connection = connURL();
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                string query = "Select ID from Users where Username = @User";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@User", user);
                conn.Open();
                object userid = command.ExecuteScalar();
                UserID.Text = userid.ToString();
                conn.Close();
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error getting the userID please diagnose this error: " + e.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        //get the question
        protected void getQuestion()
        {
            string connection = connURL();
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                string query = "Select Body from Question_List where Question_ID = @Question_ID";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Question_ID", QuestionID.Text.ToString());
                conn.Open();
                object question = command.ExecuteScalar();
                Question.Text = question.ToString();
                conn.Close();
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error getting the userID please diagnose this error: " + e.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            GridViewRow gridViewRow = (GridViewRow)button.NamingContainer;
            //Now we have the row where the button was submitted
            int index = gridViewRow.RowIndex;
            checkAnswer(index);
            //We want to check if that was the right answer OR the highest score (if there is no right answer)

        }

        protected void updateCorrectAnswer()
        {
            string connection = connURL();

            string query = "Update Question_List set Num_of_Guesses = Num_of_Guesses + 1, Is_Answered = 1, Answered_By = @UserID, Answer_Date = GETDATE() where Question_ID = @Question";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                //Update the Questions_List table to give them the owner id
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@UserID", UserID.Text.ToString());
                command.Parameters.AddWithValue("@Question", QuestionID.Text.ToString());
                conn.Open();
                command.ExecuteNonQuery();
                //Close the connection
                conn.Close();
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error updating the question: " + e.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void updateIncorrectQuestion()
        {
            string connection = connURL();

            string query = "Update Question_List set Num_of_Guesses = Num_of_Guesses + 1 where Question_ID = @Question";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                //Update the Questions_List table to give them the owner id
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Question", QuestionID.Text.ToString());
                conn.Open();
                command.ExecuteNonQuery();
                //Close the connection
                conn.Close();
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error updating the question: " + e.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void updateIncorrectAnswer(string answerid)
        {
            string connection = connURL();

            string query = "Update Questions set button_disabled = 1 where Answer_ID = @Answer_ID";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Answer_ID", answerid.ToString());
                conn.Open();
                command.ExecuteNonQuery();
                //Close the connection
                conn.Close();
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error updating the question: " + e.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        protected int ReturnNumOfGuesses()
        {
            string connection = connURL();
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                string query = "Select Num_Of_Guesses from Question_List where Question_ID = @Question_ID";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Question_ID", QuestionID.Text.ToString());
                conn.Open();
                object question = command.ExecuteScalar();
                conn.Close();
                return Convert.ToInt32(question.ToString());
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error getting the userID please diagnose this error: " + e.ToString();
                return 99;
            }
            finally
            {
                conn.Close();
            }
        }

        protected int totalScore(int guesses)
        {
            string connection = connURL();
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                string query = "Select Guess_Score from Guess_Score where Num_Of_Guesses = @guesses";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@guesses", guesses);
                conn.Open();
                object guess = command.ExecuteScalar();
                conn.Close();
                return Convert.ToInt32(guess.ToString());
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error getting the userID please diagnose this error: " + e.ToString();
                return 99;
            }
            finally
            {
                conn.Close();
            }
        }

        protected void updateUserScore()
        {
            string connection = connURL();
            int guesses = ReturnNumOfGuesses();

            //if its less than 6 update the users total score
            if (guesses < 6)
            {
                int score_total = totalScore(guesses);
                string query = "Update Users set Total_Score = Total_Score + @Score where ID = @UserID";
                //Set up the connection string
                SqlConnection conn = new SqlConnection(connection);
                try
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@UserID", UserID.Text.ToString());
                    command.Parameters.AddWithValue("@Score", score_total);
                    conn.Open();
                    command.ExecuteNonQuery();
                    //Close the connection
                    conn.Close();
                }
                catch (Exception e)
                {
                    ErrorMsg.Visible = true;
                    ErrorMsg.Text = "There was an error updating the question: " + e.ToString();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        protected void addCorrectAnswers()
        {
            string connection = connURL();

            string query = "Update Users set Correct_Answers = Correct_Answers + 1 where ID = @UserID";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@UserID", UserID.Text.ToString());
                conn.Open();
                command.ExecuteNonQuery();
                //Close the connection
                conn.Close();
            }
            catch (Exception e)
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "There was an error updating the question: " + e.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void checkAnswer(int index)
        {
            int row = index;
            string connection = connURL();
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                GridViewRow viewRow = CommentGridfield.Rows[row];
                //we Have the answerid now to check if that was the right answer.
                string answerID = viewRow.Cells[0].Text.ToString();

                string query = "Select TOP(1) Answer_ID, MAX(Score_Of_Answer) from Questions where Question_ID = @Question_ID OR (Question_ID = @Question_ID AND Correct_Answer = 1) group by Answer_ID,Correct_Answer, Score_Of_Answer order by Correct_Answer DESC, Score_Of_Answer DESC ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Question_ID", QuestionID.Text.ToString());
                conn.Open();
                object question = command.ExecuteScalar();
                string correctAnwser = question.ToString();
                if(correctAnwser.Equals(answerID))
                {
                    //Update the number of guesses as well as update the user score
                    updateCorrectAnswer();
                    updateUserScore();
                    addCorrectAnswers();
                    Response.Write("<script>alert('That is the correct answer!');</script>");
                }
                else
                {
                    updateIncorrectQuestion();
                    updateIncorrectAnswer(answerID);

                    Response.Write("<script>alert('That is not the correct answer, please try again...');</script>");
                }
            }
            catch(Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
        }
    }
}