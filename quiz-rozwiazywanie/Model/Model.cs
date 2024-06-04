namespace quiz_rozwiazywanie.Model
{
    using DAL.Encje;
    using DAL.Repozytoria;
    using System.Collections.ObjectModel;

    class Model
    {
        public ObservableCollection<Quiz> Quizzes { get; set; } = new ObservableCollection<Quiz>();
        public ObservableCollection<Pytanie> Questions { get; set; } = new ObservableCollection<Pytanie>();
        //public ObservableCollection<Odpowiedz> Answers { get; set; } = new ObservableCollection<Odpowiedz>();

        public Model() 
        {
            var quiz = RepoQuizy.getAllQuizzes();
            foreach(var q in quiz)
            {Quizzes.Add(q);}

            var question = RepoPytania.getAllQuestions();
            foreach(var q in question) { Questions.Add(q); }
        }

        public ObservableCollection<Pytanie> GetQuestionsForQuiz(Quiz quiz)
        {
            var questions = new ObservableCollection<Pytanie>();
            var question = RepoPytania.getAllQuestions();
            foreach(var q in Questions)
            {
                if(q.Id_quiz == quiz.Id)
                    questions.Add(q);
            }
            return questions;
        }

        private Pytanie findQuestionbyId(sbyte id)
        {
            foreach(var q in Questions)
            {
                if(q.Id == id)
                    return q;
            }
            return null;
        }

        public ObservableCollection<Odpowiedz> GetAnswersForQuestion(Pytanie question)
        {
            var answers = new ObservableCollection<Odpowiedz>();
            var answer = RepoOdpowiedzi.GetAllAnswers();
            foreach(var a in answer)
            {
                if (a.Id_q == question.Id)
                    answers.Add(a);
            }
            return answers;
        }

        public bool IsQuizInRepo(Quiz quiz) => Quizzes.Contains(quiz);

        public bool IsQuestionQuizInRepo(Pytanie question, Quiz quiz) => GetQuestionsForQuiz(quiz).Contains(question);

        public bool AddQuizToBase(Quiz quiz)
        {
            if (!IsQuizInRepo(quiz))
            {
                if(RepoQuizy.AddQuizToBase(quiz))
                {
                    Quizzes.Add(quiz);
                    return true;
                }
            }
            return false;
        }

        public bool AddQuestionToBase(Pytanie question, Quiz quiz)
        {
            if(!IsQuestionQuizInRepo(question, quiz))
            {
                if(RepoPytania.addQuestionToBase(question))
                {
                    return true;
                }
            }
            return false;
        }

        public bool RemoveQuizFromBase(Quiz quiz)
        {
            if (IsQuizInRepo(quiz))
            {
                Quizzes.Remove(quiz);
                RepoQuizy.RemQuizFromBase(quiz); // Call the method to remove the quiz from the database
                return true;
            }
            return false;
        }
    }
}