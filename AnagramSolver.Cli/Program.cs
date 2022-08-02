using AnagramSolver.BusinessLogic;
using AnagramSolver.Cli;


WordRepository wordRepository = new WordRepository();
wordRepository.LoadDictionary();
Settings.LoadSettings();

AnagramSolver.BusinessLogic.AnagramSolver anagramSolver = new AnagramSolver.BusinessLogic.AnagramSolver(wordRepository);

UI appUI = new UI(anagramSolver);

