using MySql.Data.MySqlClient;

namespace quiz_rozwiazywanie.DAL.Encje
{
    class Pytanie
    {
        public sbyte? Id { get; set; }
        public sbyte Id_quiz { get; set; }
        public string Content { get; set; }

        public Pytanie(MySqlDataReader reader) 
        {
            this.Id = sbyte.Parse(reader["id"].ToString());
            this.Id_quiz = sbyte.Parse(reader["id_quiz"].ToString());
            this.Content = reader["content"].ToString();
        }

        public Pytanie(sbyte id_quiz, string content) 
        {
            this.Id_quiz = id_quiz;
            this.Content = content;
        }

        public Pytanie(Pytanie pytanie) 
        {
            this.Id = pytanie.Id;
            this.Id_quiz = pytanie.Id_quiz;
            this.Content = pytanie.Content;
        }

        public override string ToString() { return Content; }
        public string insert() { return Id_quiz.ToString() + ", "+ Content; }

        public override bool Equals(object? obj)
        {
            var other = obj as Pytanie;
            if (other == null) return false;
            else if (other.Id_quiz == this.Id_quiz && other.Content == this.Content) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
