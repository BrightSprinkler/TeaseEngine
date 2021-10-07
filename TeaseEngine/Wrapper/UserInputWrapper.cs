using System;
using TeaseEngine.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class UserInputWrapper
    {
        private UserInput UserInput { get; set; }
        private Logger Logger { get; } = App.Logging.GetLogger<UserInputWrapper>();

        public UserInputWrapper(UserInput userInput)
        {
            UserInput = userInput;
        }

        public T Get<T>(Action<string> onWrongInput = null, Func<T, bool> validator = null)
        {
            Logger.Debug($"Getting input of {typeof(T).Name}");

            return UserInput.GetInput<T>(onWrongInput, validator);
        }

    }
}
