using System;

#pragma warning disable 1591

namespace NppScripts
{
    partial class Plugin
    {
        #region " Fields "

        public static NppData NppData;

        public static FuncItems FuncItems = new FuncItems();

        #endregion " Fields "

        #region " Helper "

        public static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
        }

        public static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut)
        {
            SetCommand(index, commandName, functionPointer, shortcut, false);
        }

        public static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, bool checkOnInit)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
        }

        public static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut, bool checkOnInit)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
            if (shortcut._key != 0)
                funcItem._pShKey = shortcut;
            funcItem._init2Check = checkOnInit;
            FuncItems.Add(funcItem);
        }

        public static IntPtr GetCurrentScintilla()
        {
            int curScintilla;
            Win32.SendMessage(NppData._nppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
            return (curScintilla == 0) ? NppData._scintillaMainHandle : NppData._scintillaSecondHandle;
        }

        #endregion " Helper "
    }
}