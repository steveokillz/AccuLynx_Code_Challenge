using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web;

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
            GetStackQuestions();
            fillGridview();
            updateQuestionList();
            getQuestion();
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
                string query = "If not exists(Select 1 from Questions where Answer_ID = @Answer_ID) Insert into dbo.Questions Values (@Question_ID, @Answer_ID, @Question_Information, @User_ID, @Correct_Answer, @Score_Of_Answer)";
                //Set up the connection string
                SqlConnection conn = new SqlConnection(connection);
                try
                {
                    //To help DB load i will store the correct answer and the score of answer in a hidden label.
                    
                    int highScore = 0;
                    string ans_ID = "";
                    bool correct_ans = false;
                    foreach (Item stacklist in obj.Items)
                    {
                        SqlCommand command = new SqlCommand(query, conn);
                        command.Parameters.AddWithValue("@Question_ID", QuestionID.Text.ToString());
                        command.Parameters.AddWithValue("@Answer_ID", stacklist.answer_id);
                        command.Parameters.AddWithValue("@Question_Information", stacklist.Body);
                        command.Parameters.AddWithValue("@User_ID", UserID.Text.ToString());
                        command.Parameters.AddWithValue("@Correct_Answer", stacklist.is_accepted);
                        command.Parameters.AddWithValue("@Score_Of_Answer", stacklist.Score);
                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();

                        int highScorenew = Convert.ToInt32(stacklist.Score);
                        string ans_IDnew = stacklist.answer_id.ToString();
                        bool correct_ansnew = Convert.ToBoolean(stacklist.is_accepted);

                        if(highScorenew > highScore)
                        {
                            highScore = highScorenew;
                            ans_ID = ans_IDnew;
                            correct_ans = correct_ansnew;
                        }
                    }
                    if (correct_ans == false)
                    {
                        IsCorrect.Visible = true;
                        IsCorrect.Text = "NOTE: There is no correct answer in this. In this case the correct answer will be the best score";
                    }
                    AnswerID.Text = ans_ID.ToString();
                    TotalScore.Text = highScore.ToString();
                    IsCorrect.Text = correct_ans.ToString();
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

            string query = "Select Answer_ID, Question_Information, Correct_Answer, Score_Of_Answer from Questions where Question_ID = @Question_ID ";
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

        protected void Submit_Click(object sender, EventArgs e)
        {
            //check to see if the answer is correct OR if the guess score is the highest

        }
    }
}