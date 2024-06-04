namespace quiz_rozwiazywanie.ViewModel
{
    using BaseClass;
    using quiz_rozwiazywanie.DAL.Encje;
    using quiz_rozwiazywanie.Model;
    using System.Collections.ObjectModel;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;

    class QuizzesViewModel : ViewModelBase
    {
        private Model model = null;
        private int selectedQuiz = -1;
        private ObservableCollection<Odpowiedz> answerContent;
        private Quiz currentQuiz;
        private ObservableCollection<Pytanie> questions = new ObservableCollection<Pytanie>();
        private static Timer timer;
        private static bool isRun = false;
        private int questionIndex = 0;
        private Pytanie currentQuestion;
        private ObservableCollection<bool> answersChecked = new ObservableCollection<bool>();
        private int answerIndex = 0;
        private bool answerChecked0 = false;
        private bool answerChecked1 = false;
        private bool answerChecked2 = false;
        private bool answerChecked3 = false;
        private bool enableListBox = true;
        private bool enableCheckBoxes = false;
        private ObservableCollection<Odpowiedz> answers = new ObservableCollection<Odpowiedz>();
        private int points = 0;
        private int totalPoints = 0;

        public int QuestionIndex { get { return questionIndex; } set { questionIndex = value; onPropertyChanged(nameof(QuestionIndex)); } }
        public Quiz CurrentQuiz { get { return currentQuiz; } set { currentQuiz = value; onPropertyChanged(nameof(CurrentQuiz)); Questions = model.GetQuestionsForQuiz(CurrentQuiz); AnswersChecked.Clear(); for (int i = 0; i < (questions.Count * 4); i++) answersChecked.Add(false); } }
        public Pytanie CurrentQuestion { get { return currentQuestion; } set { currentQuestion = value; onPropertyChanged(nameof(CurrentQuestion)); } }
        public ObservableCollection<Quiz> Quizzes { get; set; }
        public ObservableCollection<Pytanie> Questions { get { return questions; } set { questions = value; onPropertyChanged(nameof(Questions)); } }
        public ObservableCollection<Odpowiedz> Answers { get; set; }
        public QuizzesViewModel(Model model) 
        {
            this.model = model;
            Quizzes = model.Quizzes;
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            //Questions = model.Questions;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Time = Time.Add(TimeSpan.FromSeconds(1));
        }

        private TimeSpan time = new(0, 0, 0);
        public TimeSpan Time
        {
            get => time;
            set
            {
                time = value;
                onPropertyChanged(nameof(Time));
            }
        }

        public int SelectedQuiz
        {
            get { return selectedQuiz; }
            set
            {
                selectedQuiz = value;
                onPropertyChanged(nameof(SelectedQuiz));
            }
        }

        public ObservableCollection<Odpowiedz> AnswerContent
        { get { return answerContent; } set {  answerContent = value; onPropertyChanged(nameof(AnswerContent)); } }

        public ObservableCollection<bool> AnswersChecked { get { return answersChecked; } set { answersChecked =  value; onPropertyChanged(nameof(AnswersChecked)); } }
        public int AnswerIndex { get { return answerIndex; } set { answerIndex = value; onPropertyChanged(nameof(AnswerIndex)); } }
        public bool AnswerChecked0 { get { return answerChecked0; } set { answerChecked0 = value; onPropertyChanged(nameof(AnswerChecked0)); } }
        public bool AnswerChecked1 { get { return answerChecked1; } set { answerChecked1 = value; onPropertyChanged(nameof(AnswerChecked1)); } }
        public bool AnswerChecked2 { get { return answerChecked2; } set { answerChecked2 = value; onPropertyChanged(nameof(AnswerChecked2)); } }
        public bool AnswerChecked3 { get { return answerChecked3; } set { answerChecked3 = value; onPropertyChanged(nameof(AnswerChecked3)); } }
        public bool EnableListBox { get { return enableListBox; } set { enableListBox = value; onPropertyChanged(nameof(EnableListBox)); } }
        public bool EnableCheckBoxes { get { return enableCheckBoxes; } set { enableCheckBoxes = value; onPropertyChanged(nameof(EnableCheckBoxes)); } }

        private ICommand startQuiz = null;
        public ICommand StartQuiz
        {
            get
            {
                if (startQuiz == null)
                {
                    startQuiz = new RelayCommand(
                    arg =>
                    {
                        if (questions.Any())
                        {
                            Time = TimeSpan.Zero;
                            timer?.Start();
                            isRun = !isRun;
                            EnableListBox = false;
                            EnableCheckBoxes = true;
                            questionIndex = 0;
                            answerIndex = 0;
                            CurrentQuestion = questions[questionIndex];
                            AnswerContent = model.GetAnswersForQuestion(CurrentQuestion);
                            AnswersChecked.Clear();
                            for (int i = 0; i < Questions.Count*4; i++) AnswersChecked.Add(false);
                            answers.Clear();
                            points = 0;
                            totalPoints = 0;
                            //MessageBox.Show($"{Time.Add(TimeSpan.FromSeconds(1))}\n{Time.Add(TimeSpan.FromSeconds(1))}");
                        }
                        else
                        {
                            MessageBox.Show("Quiz nie zawiera żadnych pytań.", "Nieprawidłowy quiz", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    ,
                    arg => !isRun
                    );
                }
                return startQuiz;
            }

        }

        private ICommand stopQuiz = null;
        public ICommand StopQuiz
        {
            get
            {
                if (stopQuiz == null)
                {
                    stopQuiz = new RelayCommand(
                    arg =>
                    {
                        timer?.Stop();
                        isRun = !isRun;
                        EnableListBox = true;
                        EnableCheckBoxes = false;
                        CurrentQuestion = null;
                        AnswerContent = null;
                        AnswersChecked[AnswerIndex] = answerChecked0;
                        AnswersChecked[AnswerIndex + 1] = answerChecked1;
                        AnswersChecked[AnswerIndex + 2] = answerChecked2;
                        AnswersChecked[AnswerIndex + 3] = answerChecked3;
                        AnswerChecked0 = false;
                        AnswerChecked1 = false;
                        AnswerChecked2 = false;
                        AnswerChecked3 = false;
                        foreach (var q in questions)
                        {
                            ObservableCollection<Odpowiedz> answer = model.GetAnswersForQuestion(q);
                            foreach (var a in answer) answers.Add(a);
                        }
                        for (int i = 0; i < answers.Count; i++)
                        {
                            if (answersChecked[i] == answers[i].Correct && answersChecked[i]) points += 1;
                            else if (answersChecked[i] != answers[i].Correct && answersChecked[i]) points -= 1;
                            if (answers[i].Correct) totalPoints += 1;
                        }
                        MessageBox.Show($"Ilość punktów: {points}/{totalPoints}\nCzas: {Time}");
                        Time = TimeSpan.Zero;
                    }
                    ,
                    arg => isRun
                    );
                }
                return stopQuiz;
            }

        }

        private ICommand nextQuestion = null;
        public ICommand NextQuestion
        {
            get
            {
                if (nextQuestion == null)
                {
                    nextQuestion = new RelayCommand(
                    arg =>
                    {
                        if (questionIndex < questions.Count - 1)
                        {
                            questionIndex++;
                            CurrentQuestion = questions[questionIndex];
                            AnswerContent = model.GetAnswersForQuestion(CurrentQuestion);
                            AnswersChecked[AnswerIndex] = answerChecked0;
                            AnswersChecked[AnswerIndex + 1] = answerChecked1;
                            AnswersChecked[AnswerIndex + 2] = answerChecked2;
                            AnswersChecked[AnswerIndex + 3] = answerChecked3;
                            answerIndex += 4;
                            AnswerChecked0 = answersChecked[answerIndex];
                            AnswerChecked1 = answersChecked[answerIndex + 1];
                            AnswerChecked2 = answersChecked[answerIndex + 2];
                            AnswerChecked3 = answersChecked[answerIndex + 3];
                        }
                    }
                    ,
                    arg => isRun
                    );
                }
                return nextQuestion;
            }

        }
        private ICommand prevQuestion = null;
        public ICommand PrevQuestion
        {
            get
            {
                if (prevQuestion == null)
                {
                    prevQuestion = new RelayCommand(
                    arg =>
                    {
                        if (questionIndex > 0)
                        {
                            questionIndex--;
                            CurrentQuestion = questions[questionIndex];
                            AnswerContent = model.GetAnswersForQuestion(CurrentQuestion);
                            AnswersChecked[AnswerIndex] = answerChecked0;
                            AnswersChecked[AnswerIndex + 1] = answerChecked1;
                            AnswersChecked[AnswerIndex + 2] = answerChecked2;
                            AnswersChecked[AnswerIndex + 3] = answerChecked3;
                            AnswerIndex -= 4;
                            AnswerChecked0 = answersChecked[answerIndex];
                            AnswerChecked1 = answersChecked[answerIndex + 1];
                            AnswerChecked2 = answersChecked[answerIndex + 2];
                            AnswerChecked3 = answersChecked[answerIndex + 3];
                        }
                    }
                    ,
                    arg => isRun
                    );
                }
                return prevQuestion;
            }
        }

    }
}
