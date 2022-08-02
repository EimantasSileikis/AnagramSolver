using AnagramSolver.BusinessLogic;
using AnagramSolver.Cli;


WordRepository wordRepository = new WordRepository();
wordRepository.LoadDictionary();

AnagramSolver.BusinessLogic.AnagramSolver anagramSolver = new AnagramSolver.BusinessLogic.AnagramSolver(wordRepository);

UI appUI = new UI(anagramSolver);