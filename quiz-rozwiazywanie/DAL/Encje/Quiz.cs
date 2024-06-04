using MySql.Data.MySqlClient;

namespace quiz_rozwiazywanie.DAL.Encje
{
    class Quiz
    {
        public sbyte? Id { get; set; }
        public string Name { get; set; }

        public Quiz(MySqlDataReader reader)
        {
            Id = sbyte.Parse(reader["id"].ToString());
            Name = reader["name"].ToString();
        }

        public Quiz(string name)
        {
            this.Name = name;
            this.Id = null;
        }

        public Quiz(Quiz quiz)
        {
            this.Id = quiz.Id;
            this.Name = quiz.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public string insert() { return ToString(); }

        public override bool Equals(object? obj)
        {
            var quiz = obj as Quiz;
            if (quiz is null) { return false; }
            else if (this.Name == quiz.Name) { return true; }
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
