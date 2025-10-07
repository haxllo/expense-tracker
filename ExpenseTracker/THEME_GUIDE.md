# V0.DEV Inspired Modern Theme - Implementation Guide

## Overview
This theme provides a clean, professional, and modern dark UI inspired by v0.dev and Linear.app, designed for WPF applications.

## Color Palette

### Background
- **AppBackground**: `#0F1118` - Main application background
- **Surface**: `#1B1E26` - Card and panel backgrounds
- **SurfaceHover**: `#22252E` - Hover state for interactive surfaces
- **SurfaceActive**: `#2A2D36` - Active/pressed state

### Accents
- **AccentMint**: `#4FD1C5` - Primary accent (cyan-mint)
- **AccentViolet**: `#7F7AFF` - Secondary accent (violet)
- **AccentHover**: `#63E6BE` - Brighter mint for hover states

### Text
- **TextPrimary**: `#E5E7EB` - Main text
- **TextSecondary**: `#A0AEC0` - Supporting text
- **TextTertiary**: `#6B7280` - Disabled/placeholder text

### Borders
- **Border**: `#2E3340` - Subtle borders and dividers

## Typography

### Font Families
- **Primary**: Segoe UI, Inter, Poppins, SF Pro Display, Arial
- **Mono**: Consolas, SF Mono, Monaco

### Font Sizes
- 10px - Small labels
- 11px - Table headers, stat labels
- 12px - Form labels
- 13px - Body text, buttons
- 14px - Regular inputs
- 16px - Subheadings
- 18px - Section headings
- 20px - Page subheadings
- 24px - Stats numbers, page headings
- 32px - Hero numbers

## Spacing System
Based on 8px grid:
- **4px** - Tight spacing
- **8px** - Compact spacing
- **12px** - Default padding
- **16px** - Medium spacing
- **24px** - Large spacing
- **32px** - Section spacing

## Corner Radius
- **Small**: 6px - Badges, small buttons
- **Medium**: 8px - Buttons, inputs
- **Large**: 12px - Cards, panels
- **XLarge**: 16px - Modal dialogs, dashboard cards

## Component Styles

### 1. Dashboard Card
```xaml
<Border Style="{StaticResource DashboardCard}">
    <!-- Your content -->
</Border>
```

### 2. Stats Card
```xaml
<Border Style="{StaticResource StatsCard}">
    <StackPanel>
        <TextBlock Text="TOTAL EXPENSES" Style="{StaticResource StatsLabel}"/>
        <TextBlock Text="₹45,320" Style="{StaticResource StatsNumber}"/>
    </StackPanel>
</Border>
```

### 3. Primary Button
```xaml
<Button Content="Add Expense" 
        Style="{StaticResource PrimaryButton}"
        Command="{Binding AddExpenseCommand}"/>
```

### 4. Secondary Button
```xaml
<Button Content="Refresh" 
        Style="{StaticResource SecondaryButton}"
        Command="{Binding RefreshCommand}"/>
```

### 5. Danger Button
```xaml
<Button Content="Delete" 
        Style="{StaticResource DangerButton}"
        Command="{Binding DeleteCommand}"/>
```

### 6. Text Input
```xaml
<StackPanel>
    <TextBlock Text="Amount" Style="{StaticResource ModernLabel}"/>
    <TextBox Style="{StaticResource ModernTextBox}" 
             Text="{Binding Amount}"/>
</StackPanel>
```

### 7. ComboBox
```xaml
<StackPanel>
    <TextBlock Text="Category" Style="{StaticResource ModernLabel}"/>
    <ComboBox Style="{StaticResource ModernComboBox}"
              ItemsSource="{Binding Categories}"
              SelectedItem="{Binding SelectedCategory}"/>
</StackPanel>
```

### 8. DataGrid
```xaml
<DataGrid Style="{StaticResource ModernDataGrid}"
          RowStyle="{StaticResource ModernDataGridRow}"
          ColumnHeaderStyle="{StaticResource ModernDataGridColumnHeader}"
          CellStyle="{StaticResource ModernDataGridCell}"
          ItemsSource="{Binding Expenses}">
    <DataGrid.Columns>
        <DataGridTextColumn Header="DATE" Binding="{Binding Date}"/>
        <DataGridTextColumn Header="CATEGORY" Binding="{Binding Category}"/>
        <!-- More columns -->
    </DataGrid.Columns>
</DataGrid>
```

## Example Dashboard Layout

```xaml
<Window Background="{StaticResource AppBackgroundBrush}">
    <Grid Margin="24">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/> <!-- Header -->
            <RowDefinition Height="Auto"/> <!-- Stats Cards -->
            <RowDefinition Height="Auto"/> <!-- Filters -->
            <RowDefinition Height="*"/>    <!-- Table -->
            <RowDefinition Height="Auto"/> <!-- Actions -->
        </Grid.RowDefinitions>

        <!-- Header -->
        <Grid Grid.Row="0" Margin="0,0,0,24">
            <TextBlock Text="Expense Tracker" Style="{StaticResource Heading1}"/>
            <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
                <Button Content="Add Expense" Style="{StaticResource PrimaryButton}" Margin="0,0,8,0"/>
                <Button Content="Refresh" Style="{StaticResource SecondaryButton}"/>
            </StackPanel>
        </Grid>

        <!-- Stats Cards -->
        <UniformGrid Grid.Row="1" Rows="1" Columns="5" Margin="0,0,0,24">
            <Border Style="{StaticResource StatsCard}" Margin="0,0,8,0">
                <StackPanel>
                    <TextBlock Text="TOTAL EXPENSES" Style="{StaticResource StatsLabel}"/>
                    <TextBlock Text="₹45,320" Style="{StaticResource StatsNumber}" Foreground="{StaticResource AccentMintBrush}"/>
                </StackPanel>
            </Border>
            <!-- More stats cards -->
        </UniformGrid>

        <!-- Filters -->
        <Border Grid.Row="2" Style="{StaticResource DashboardCard}" Margin="0,0,0,16">
            <Grid>
                <TextBox Style="{StaticResource ModernTextBox}" Width="200" HorizontalAlignment="Left"/>
                <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
                    <!-- Filter controls -->
                </StackPanel>
            </Grid>
        </Border>

        <!-- Table -->
        <Border Grid.Row="3" Style="{StaticResource DashboardCard}">
            <DataGrid Style="{StaticResource ModernDataGrid}"
                      RowStyle="{StaticResource ModernDataGridRow}"
                      ColumnHeaderStyle="{StaticResource ModernDataGridColumnHeader}"
                      CellStyle="{StaticResource ModernDataGridCell}"/>
        </Border>

        <!-- Actions -->
        <StackPanel Grid.Row="4" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,16,0,0">
            <Button Content="Edit" Style="{StaticResource SecondaryButton}" Margin="0,0,8,0"/>
            <Button Content="Delete" Style="{StaticResource DangerButton}"/>
        </StackPanel>
    </Grid>
</Window>
```

## Add Expense Modal Example

```xaml
<!-- Modal Overlay -->
<Grid x:Name="ModalOverlay" Background="#80000000" Visibility="Collapsed">
    <Border Style="{StaticResource DashboardCard}" 
            Width="450" 
            Height="Auto"
            VerticalAlignment="Center"
            HorizontalAlignment="Center"
            Padding="32">
        <StackPanel>
            <!-- Title -->
            <TextBlock Text="Add Expense" Style="{StaticResource Heading2}" Margin="0,0,0,24"/>
            
            <!-- Amount -->
            <StackPanel Margin="0,0,0,16">
                <TextBlock Text="Amount" Style="{StaticResource ModernLabel}"/>
                <TextBox Style="{StaticResource ModernTextBox}" Text="{Binding Amount}"/>
            </StackPanel>
            
            <!-- Category -->
            <StackPanel Margin="0,0,0,16">
                <TextBlock Text="Category" Style="{StaticResource ModernLabel}"/>
                <ComboBox Style="{StaticResource ModernComboBox}" 
                          ItemsSource="{Binding Categories}"
                          SelectedItem="{Binding SelectedCategory}"/>
            </StackPanel>
            
            <!-- Description -->
            <StackPanel Margin="0,0,0,24">
                <TextBlock Text="Description" Style="{StaticResource ModernLabel}"/>
                <TextBox Style="{StaticResource ModernTextBox}" 
                         Text="{Binding Description}"
                         Height="80"
                         TextWrapping="Wrap"
                         AcceptsReturn="True"/>
            </StackPanel>
            
            <!-- Actions -->
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>
                <Button Content="Cancel" 
                        Style="{StaticResource SecondaryButton}" 
                        Grid.Column="0" 
                        Margin="0,0,8,0"
                        Command="{Binding CancelCommand}"/>
                <Button Content="Save" 
                        Style="{StaticResource PrimaryButton}" 
                        Grid.Column="1" 
                        Margin="8,0,0,0"
                        Command="{Binding SaveCommand}"/>
            </Grid>
        </StackPanel>
    </Border>
</Grid>
```

## Integration with App.xaml

Add this to your App.xaml:

```xaml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Themes/ModernTheme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

## Design Principles

1. **Consistent Spacing**: Use the 8px grid system
2. **Clear Hierarchy**: Primary → Secondary → Tertiary colors
3. **Subtle Effects**: Soft shadows, not harsh borders
4. **Hover States**: Always provide visual feedback
5. **Typography**: Use font weights to create hierarchy
6. **Color Usage**: 
   - Mint accent for primary actions
   - Violet accent for secondary highlights
   - Red only for destructive actions
7. **Card Design**: Prefer subtle shadows over borders
8. **Minimal Decoration**: Clean, functional, uncluttered

## Tips

- Keep backgrounds dark (`#0F1118`) for comfort
- Use `#1B1E26` for elevated surfaces (cards)
- Add soft glows on interactive elements
- Maintain consistent corner radii (8-12px)
- Use uppercase for labels (11px, Medium weight)
- Keep stat numbers large and prominent (24px)
- Ensure 44px minimum click target size
- Use `TextSecondary` for supporting information
- Reserve `AccentMint` for primary CTAs

## Accessibility

- Contrast ratio: All text meets WCAG AA standards
- Focus indicators: Border changes to accent color
- Interactive elements: Minimum 44px touch target
- Semantic colors: Red for danger, mint for success

---

**Result**: A clean, professional, v0.dev-inspired dark theme that feels modern, minimal, and elegant.
