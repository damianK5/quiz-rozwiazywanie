namespace quiz_rozwiazywanie.ViewModel
{
    using quiz_rozwiazywanie.Model;
    class MainViewModel
    {
        private Model model = new Model();
        public QuizzesViewModel Qvm { get; set; }

        public MainViewModel()
        {
            Qvm = new QuizzesViewModel(model);
        }
    }
}
