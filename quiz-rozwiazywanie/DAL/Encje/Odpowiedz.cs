using MySql.Data.MySqlClient;

namespace quiz_rozwiazywanie.DAL.Encje
{
    class Odpowiedz
    {
        public sbyte? Id {get;set;}
        public sbyte Id_q { get;set;}
        public string Answer { get;set;}
        public bool Correct { get;set;}

        public Odpowiedz(MySqlDataReader reader)
        {
            this.Id = sbyte.Parse(reader["id"].ToString());
            this.Id_q = sbyte.Parse(reader["id_q"].ToString());
            this.Answer = reader["answer"].ToString();
            string cor = reader["correct"].ToString();
            if (cor == "1") { this.Correct = true; }
            else { this.Correct = false; }
        }

        public Odpowiedz(sbyte id_q, string answer, bool correct)
        {
            this.Id_q = id_q;
            this.Answer= answer;
            this.Correct = correct;
        }

        public Odpowiedz(Odpowiedz odp) 
        {
            this.Id = odp.Id;
            this.Id_q = odp.Id_q;
            this.Answer = odp.Answer;
            this.Correct = odp.Correct;
        }

        public override string ToString() { return Answer; }
        public string insert() {return Id_q.ToString()+", "+Answer+", "+Correct.ToString();}
        public override bool Equals(object? obj)
        {
            var qs = obj as Odpowiedz;
            if (qs == null) return false;
            else if (this.Id_q == qs.Id_q && this.Answer==qs.Answer && this.Correct == qs.Correct) return true;
            else return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
