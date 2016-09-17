
namespace BitmapCompressor.Console.Utilities
{
    /// <summary>
    /// Provides input handling for the console application.
    /// </summary>
    public interface IInputSystem
    {
        /// <summary>
        /// Prompt the user for a Yes or No input. Returns true if
        /// the user selected Yes and false if the user selected No.
        /// </summary>
        bool PromptYesOrNo();
    }
}
