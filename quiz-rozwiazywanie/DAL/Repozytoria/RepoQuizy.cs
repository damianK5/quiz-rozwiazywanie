using MySql.Data.MySqlClient;
using quiz_rozwiazywanie.DAL.Encje;

namespace quiz_rozwiazywanie.DAL.Repozytoria
{
    static class RepoQuizy
    {
        private const string ALL_QUIZZES = "SELECT * FROM quizy";
        private const string ADD_QUIZ = "INSERT INTO quizy(name) VALUES";
        
        public static List<Quiz> getAllQuizzes()
        {
            List<Quiz> quizzes = new List<Quiz>();

            using (var connection = DBConnection.Instance.Connection) 
            {
                MySqlCommand command = new MySqlCommand(ALL_QUIZZES, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read()) { quizzes.Add(new Quiz(reader)); }
                connection.Close();
            }
            return quizzes;
        }

        public static bool AddQuizToBase(Quiz quiz)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                string quizName = MySqlHelper.EscapeString(quiz.insert());
                MySqlCommand command = new MySqlCommand($"{ADD_QUIZ} ('{quizName}')", connection);
                connection.Open();
                var n = command.ExecuteNonQuery();
                state = true;
                quiz.Id = (sbyte)command.LastInsertedId;
                connection.Close();
            }
            return state;
        }

        public static bool RemQuizFromBase(Quiz quiz)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                string removal = $"DELETE FROM odpowiedzi where id_q in(select id from pytania where id_quiz = '{quiz.Id}')" +
                $"DELETE FROM pytania where id_quiz = '{quiz.Id}'" +
                $"DELETE FROM quizy where id = '{quiz.Id}'";

                MySqlCommand command = new MySqlCommand(removal);
                connection.Open();
                var n = command.ExecuteNonQuery();
                state = true;
                connection.Close();

            }
            return state;
        }

    }
}
