using System;
using System.Collections.ObjectModel;
using Clac.UI.Configuration;
using Clac.UI.Models;

namespace Clac.UI.Services;

public class DisplayService
{
    private readonly UISettings _settings;

    public DisplayService(UISettings settings)
    {
        _settings = settings;
    }

    public ObservableCollection<StackLineItem> CreateDisplayItems(string[] stack)
    {
        var items = new ObservableCollection<StackLineItem>();
        int displayLines = _settings.DisplayLines;
        int totalLines = Math.Max(displayLines, stack.Length);

        for (int i = 0; i < totalLines; i++)
            items.Add(new StackLineItem("", ""));

        return items;
    }

    public bool ShouldShowScrollBar(string[] stack)
    {
        return stack.Length > _settings.DisplayLines;
    }
}