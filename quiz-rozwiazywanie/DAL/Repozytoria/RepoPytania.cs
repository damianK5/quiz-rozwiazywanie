using MySql.Data.MySqlClient;
using quiz_rozwiazywanie.DAL.Encje;

namespace quiz_rozwiazywanie.DAL.Repozytoria
{
    static class RepoPytania
    {
        private const string ALL_QUESTIONS = "SELECT * FROM pytania";
        private const string ADD_QUESTION = "INSERT INTO 'pytania'('id_quiz', 'content') VALUES ";

        public static List<Pytanie> getAllQuestions()
        {
            List<Pytanie> questions = new List<Pytanie>();

            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(ALL_QUESTIONS, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read()) { questions.Add(new Pytanie(reader)); }
                connection.Close();
            }
            return questions;
        }

        public static bool addQuestionToBase(Pytanie question) 
        {
            bool state = false;
            using (var  connection = DBConnection.Instance.Connection) 
            {
                MySqlCommand command = new MySqlCommand($"{ADD_QUESTION} ({question.insert()})");
                connection.Open();
                var n = command.ExecuteNonQuery();
                state = true;
                question.Id = (sbyte)command.LastInsertedId;
                connection.Close();
            }
            return state;
        }

        public static bool remQuestionFromBase(Pytanie question) 
        {
            bool state = false;
            using ( var connection = DBConnection.Instance.Connection)
            {
                string str = $"DELETE FROM odpowiedzi where id_q = {question.Id}" +
                $"DELETE FROM pytania where id = '{question.Id}'";
                MySqlCommand command = new MySqlCommand(str);
                connection.Open();
                var n = command.ExecuteNonQuery();
                state = true;
                connection.Close();
            }
            return state;
        }


    }
}
