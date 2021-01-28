using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input; //for RoutedUICommand

namespace GraforWpfDll 
{
    public static class AllCustomCommands
    {
        public static RoutedUICommand ExitCommand;
        public static RoutedUICommand WordWrapCommand;
        public static RoutedUICommand SaveChangesCommand;
        public static RoutedUICommand ReplaceCommand;

        static AllCustomCommands()
        {
            // Close Application at Alt+F4
            InputGestureCollection exitInputs = new InputGestureCollection();
            exitInputs.Add(new KeyGesture(Key.F4, ModifierKeys.Alt));
            ExitCommand = new RoutedUICommand("Exit application", 
                                              "ExitApplication",
                                               typeof(AllCustomCommands), exitInputs);

            WordWrapCommand = new RoutedUICommand("Word wrap", 
                                                  "WordWrap",
                                                  typeof(AllCustomCommands));

            SaveChangesCommand = new RoutedUICommand("Save changes", 
                                                     "SaveChanges",
                                                     typeof(AllCustomCommands));

            ReplaceCommand = new RoutedUICommand("Replace month's photos", 
                                                 "Replace",
                                                 typeof(AllCustomCommands));
        }
    }
}
