# JetBrains Mono Font Integration

## Overview

This application uses **JetBrains Mono** font for a modern, developer-friendly aesthetic.

## Font Family Configuration

### Primary Font: **JetBrains Mono**
- Used for all body text, headings, and UI elements
- Variable: `PrimaryFont` in `ModernTheme.xaml`
- Fallback chain: `JetBrains Mono → Consolas → Segoe UI → system-ui → Arial`

### Monospace Font: **JetBrains Mono**
- Available for code or monospace text
- Variable: `MonoFont` in `ModernTheme.xaml`
- Fallback chain: `JetBrains Mono → Consolas → SF Mono → Monaco → Courier New`

## Installation

### Option 1: Install System-Wide (Recommended)

1. Download JetBrains Mono from the official website:
   - Visit: https://www.jetbrains.com/lp/mono/
   - Click "Download font" button
   - Or visit: https://github.com/JetBrains/JetBrainsMono/releases/latest
   - Extract the ZIP file

2. Install the fonts on Windows:
   - Navigate to the extracted `fonts/ttf` folder
   - Look for: `JetBrainsMono-Regular.ttf`, `JetBrainsMono-Medium.ttf`, `JetBrainsMono-SemiBold.ttf`, `JetBrainsMono-Bold.ttf`
   - **Select all the JetBrainsMono font files** you want to install
   - **Right-click** on the selected files
   - Choose **"Install for all users"** (requires admin) or **"Install"** (current user only)
   - Restart the ExpenseTracker application

### Option 2: Use Fallback Fonts

If JetBrains Mono is not installed, the application will automatically fall back to:
- **Consolas** (Windows system font) - excellent monospace fallback
- **Segoe UI** (Windows system font) - clean sans-serif fallback

## Font Weights Used

- **Regular (400)**: Body text, labels
- **Medium (500)**: Secondary headings, buttons
- **SemiBold (600)**: Headings, stats values, important text
- **Bold (700)**: Major headings (if needed)

## Font Characteristics

### JetBrains Mono
- **Style**: Monospaced typeface
- **Best For**: UI text, numbers, data, code-like aesthetics
- **Characteristics**: 
  - Increased letter height for better readability
  - Distinct character shapes (no confusion between I, l, 1)
  - Designed specifically for developers
  - Clean, modern appearance
  - Excellent for financial/numeric data

## Resources

- **Official Website**: https://www.jetbrains.com/lp/mono/
- **GitHub Repository**: https://github.com/JetBrains/JetBrainsMono
- **License**: OFL-1.1 (free for personal and commercial use)

## Notes

- The application is designed to work with or without JetBrains Mono installed
- All font references use the `{StaticResource PrimaryFont}` and `{StaticResource MonoFont}` resource keys
- Font configuration is centralized in `Themes/ModernTheme.xaml`
- JetBrains Mono works excellently for expense tracking apps due to clear number distinction
