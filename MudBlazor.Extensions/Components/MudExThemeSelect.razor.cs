﻿using Microsoft.AspNetCore.Components;
using MudBlazor.Extensions.Core;
using MudBlazor.Extensions.Helper;
using Nextended.Core.Extensions;

namespace MudBlazor.Extensions.Components;

/// <summary>
/// Component to select a theme from a list of themes.
/// </summary>
public partial class MudExThemeSelect<TTheme>
{
    private ThemePreset<TTheme> _selected;
    private MudSelect<ThemePreset<TTheme>> _mudSelector;

    /// <summary>
    /// Variant if SelectionMode is ThemeSelectionMode.Select
    /// </summary>
    [Parameter] public Variant SelectVariant { get; set; } = Variant.Outlined;

    /// <summary>
    /// Label
    /// </summary>
    [Parameter] public string Label { get; set; } = "Themes";

    /// <summary>
    /// Style for the select component.
    /// </summary>
    [Parameter] public string Style { get; set; } = "min-width: 350px";

    /// <summary>
    /// Class for the select component.
    /// </summary>
    [Parameter] public string Class { get; set; }

    /// <summary>
    /// Style for one theme item
    /// </summary>
    [Parameter] public string ItemStyle { get; set; }

    /// <summary>
    /// Class for one theme item
    /// </summary>
    [Parameter] public string ItemClass { get; set; }

    /// <summary>
    /// Gets or sets the selection mode of the component with a default value of <see cref="ThemeSelectionMode.Select"/>.
    /// </summary>
    [Parameter] public ThemeSelectionMode SelectionMode { get; set; } = ThemeSelectionMode.Select;

    /// <summary>
    /// Gets or sets the preview mode of the component with a default value of <see cref="ThemePreviewMode.BothDiagonal"/>.
    /// </summary>
    [Parameter] public ThemePreviewMode PreviewMode { get; set; } = ThemePreviewMode.BothDiagonal;

    /// <summary>
    /// Gets or sets the available themes.
    /// </summary>
    [Parameter] public ICollection<ThemePreset<TTheme>> Available { get; set; }

    /// <summary>
    /// Gets or sets the currently selected theme.
    /// </summary>
    [Parameter]
    public ThemePreset<TTheme> Selected
    {
        get => _selected;
        set
        {
            if (!EqualityComparer<ThemePreset<TTheme>>.Default.Equals(_selected, value))
            {
                _selected = value;
                RaiseChanged();
            }
        }
    }

    private void RaiseChanged()
    {
        SelectedChanged.InvokeAsync(Selected);
        SelectedThemeChanged.InvokeAsync(SelectedTheme);
        SelectedValueChanged.InvokeAsync(SelectedValue);
    }

    /// <summary>
    /// Event that is raised when the <see cref="Selected"/> property is changed.
    /// </summary>
    [Parameter] public EventCallback<ThemePreset<TTheme>> SelectedChanged { get; set; }

    /// <summary>
    /// Event that is raised when the <see cref="SelectedTheme"/> property is changed.
    /// </summary>
    [Parameter] public EventCallback<TTheme> SelectedThemeChanged { get; set; }

    /// <summary>
    /// Event that is raised when the <see cref="SelectedValue"/> property is changed.
    /// </summary>
    [Parameter] public EventCallback<object> SelectedValueChanged { get; set; }

    /// <summary>
    /// Gets or sets the selected theme ads object to allow binding to non generic components
    /// </summary>
    [Parameter]
    public object SelectedValue
    {
        get => Selected;
        set => Selected = value as ThemePreset<TTheme>;
    }

    /// <summary>
    /// Gets or sets the selected theme from the available themes.
    /// </summary>
    [Parameter]
    public TTheme SelectedTheme
    {
        get => Selected?.Theme;
        set => Selected = Available?.FirstOrDefault(x => x?.Theme == value);
    }

    /// <summary>
    /// Returns a preview image of the theme with the specified dimensions.
    /// </summary>
    private string PreviewImage(MudExDimension mudExDimension) => PreviewImage(Selected?.Theme, mudExDimension);

    /// <summary>
    /// Returns a preview image of the specified theme with the specified dimensions.
    /// </summary>
    private string PreviewImage(TTheme theme, MudExDimension mudExDimension)
    {
        if (theme is null)
            return string.Empty;
        return PreviewMode switch
        {
            ThemePreviewMode.DarkOnly => MudExSvg.ApplicationImage(theme, true, mudExDimension),
            ThemePreviewMode.LightOnly => MudExSvg.ApplicationImage(theme, false, mudExDimension),
            ThemePreviewMode.BothDiagonal => MudExSvg.ApplicationImage(theme, mudExDimension, SliceDirection.Diagonal),
            ThemePreviewMode.BothHorizontal => MudExSvg.ApplicationImage(theme, mudExDimension, SliceDirection.Horizontal),
            ThemePreviewMode.BothVertical => MudExSvg.ApplicationImage(theme, mudExDimension, SliceDirection.Vertical),
            _ => MudExSvg.ApplicationImage(theme, mudExDimension, SliceDirection.Diagonal)
        };
    }

    /// <summary>
    /// Returns a boolean indicating whether the dropdown menu of the component is open or closed.
    /// </summary>
    private bool IsOpen()
    {
        // TODO: MudSelect should expose a property for this
        return _mudSelector is null || _mudSelector.ExposeField<bool>("_isOpen");
    }

    /// <summary>
    /// Returns the style of the container of the theme name label.
    /// </summary>
    private string GetThemeNameContainerStyle()
    {
        return MudExStyleBuilder.Default
            .WithMargin("auto auto auto 25px", IsOpen())
            .WithMargin("12px 0 0 18px", !IsOpen())
            .Build();
    }
}

/// <summary>
/// Enumeration representing the preview mode of a theme.
/// </summary>
public enum ThemePreviewMode
{
    DarkOnly,
    LightOnly,
    BothDiagonal,
    BothHorizontal,
    BothVertical,
}

/// <summary>
/// Enumeration representing the selection mode of a theme.
/// </summary>
public enum ThemeSelectionMode
{
    Select,
    ItemList
}