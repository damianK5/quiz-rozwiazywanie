using MySql.Data.MySqlClient;
using quiz_rozwiazywanie.DAL.Encje;

namespace quiz_rozwiazywanie.DAL.Repozytoria
{
    static class RepoOdpowiedzi
    {
        private static string ALL_ANSWERS = "SELECT * FROM odpowiedzi";
        private static string ADD_ANSWER = "INSERT INTO 'odpowiedzi'('id_q', 'answer', 'correct') VALUES ";

        public static List<Odpowiedz> GetAllAnswers()
        {
            List<Odpowiedz> answers = new List<Odpowiedz>();
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand($"{ALL_ANSWERS} ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read()) { answers.Add(new Odpowiedz(reader)); }
                connection.Close();
            }
            return answers;
        }

        public static bool AddAnswerToBase(Odpowiedz answer)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand($"{ADD_ANSWER} {answer.insert()}");
                connection.Open();
                var n = command.ExecuteNonQuery();
                state = true;
                answer.Id = (sbyte)command.LastInsertedId;
                connection.Close();
            }
            return state;
        }
    }
}
