using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web;

namespace AccuLynx_Code_Challenge
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userMachine = HttpContext.Current.Server.MachineName;
            Label1.Text = "Welcome User: " + userMachine;
            addNewUser(userMachine);
            getUserID(userMachine);
            checkQuestionTable();

        }

        protected void addNewUser(string user)
        {
            string connection = connURL();

            //Since this is a 1 time call then we can use a sql query
            //TODO make this a stored procedure for further protection against sql injection if necessary
            string query = "If not exists(Select 1 from Users where Username = @User) Insert into Users (Username, Correct_Answers) Values (@User, 0)";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            //Insert a new user into the Users table, if they are already there then dont insert them!
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@User", user);
            conn.Open();
            command.ExecuteNonQuery();
            //Close the connection
            conn.Close();
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

        //We will use this to get the information from stack overflow
        protected void get_Stack_JSON()
        {

        }

        //This method is to check to see if we need the method GetAPICallSync to actually run during pg load 
        //(Overkill but necessary to save api calls)
        protected void checkQuestionTable()
        {
            string connection = connURL();
            string query = "Select TOP(1) Question_ID from dbo.Question_List";
            //Set up the connection string
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand command = new SqlCommand(query, conn);
            conn.Open();
            using (SqlDataReader read = command.ExecuteReader())
            {
                //This is to check if there is an actual value in here!
                if (read.HasRows.Equals(false))
                {
                    GetAPICallAsync();
                }
            }
            conn.Close();
        }
        //Main bulk of the JSON API call
        //We should only Run this once to get a list of inital data, otherwise were wasting api calls....
        protected void GetAPICallAsync()
        {
            var URL = "https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&site=stackoverflow&filter=withbody";
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


                string query = "If not exists(Select 1 from Question_List where Question_ID = @Question_ID and Current_Owner = @User) Insert into dbo.Question_List (Question_ID, Title, Body) Values (@Question_ID, @Title, @Body)";
                //Set up the connection string
                SqlConnection conn = new SqlConnection(connection);
                foreach (Item stacklist in obj.items)
                {
                    //We only want questions that are answered on the website
                    if (stacklist.is_answered == true)
                    {
                        //Insert a new user into the Users table, if they are already there then dont insert them!
                        SqlCommand command = new SqlCommand(query, conn);
                        command.Parameters.AddWithValue("@Question_ID", stacklist.question_id);
                        command.Parameters.AddWithValue("@Title", stacklist.title);
                        command.Parameters.AddWithValue("@Body", stacklist.Body);
                        command.Parameters.AddWithValue("@User", Convert.ToInt32(UserID.Text.ToString()));
                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

        }

        public class Owner
        {
            public int reputation { get; set; }
            public int user_id { get; set; }
            public string user_type { get; set; }
            public string profile_image { get; set; }
            public string display_name { get; set; }
            public string link { get; set; }
            public int? accept_rate { get; set; }
        }

        public partial class Item
        {
            public string[] Tags { get; set; }
            public Owner Owner { get; set; }
            public bool is_answered { get; set; }
            public long ViewCount { get; set; }
            public long AcceptedAnswerId { get; set; }
            public long AnswerCount { get; set; }
            public long Score { get; set; }
            public long LastActivityDate { get; set; }
            public long CreationDate { get; set; }
            public long question_id { get; set; }
            public Uri Link { get; set; }
            public string title { get; set; }
            public string Body { get; set; }
        }


        public class RootObject
        {
            public List<Item> items { get; set; }
            public bool has_more { get; set; }
            public int quota_max { get; set; }
            public int quota_remaining { get; set; }
        }
        protected void New_Quesitons_Click(object sender, EventArgs e)
        {
            //Just Recall the Api call method
            GetAPICallAsync();
            Stack_Quesitons.DataBind();
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

        protected void Get_My_Questions_Click(object sender, EventArgs e)
        {
            //Call the SQL method to get questions
            string connection = connURL();

            //Since this is a 1 time call then we can use a sql query
            //TODO make this a stored procedure for further protection against sql injection if necessary
            string query = "Select Question_ID, Title, Current_Owner from Question_List where Current_Owner = @UserID ";
            //Set up the connection string
            //We will need parameters from the Label.
            //In this case we will be using the Computer name as a userid
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@UserID", UserID.Text.ToString());
            conn.Open();
            //Databind the data into the sql statements
            SqlDataReader read = command.ExecuteReader();
            My_Questions.DataBind();
            conn.Close();
        }
    }
}