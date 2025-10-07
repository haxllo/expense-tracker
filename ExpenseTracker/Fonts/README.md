# Geist Fonts Integration

## Overview

This application uses **Geist Sans** and **Geist Mono** fonts from Vercel for a modern, clean aesthetic.

## Font Family Configuration

### Primary Font: **Geist Sans**
- Used for all body text, headings, and UI elements
- Variable: `PrimaryFont` in `ModernTheme.xaml`
- Fallback chain: `Geist → Segoe UI → Inter → system-ui → Arial`

### Monospace Font: **Geist Mono**
- Available for code or monospace text
- Variable: `MonoFont` in `ModernTheme.xaml`
- Fallback chain: `Geist Mono → Consolas → SF Mono → Monaco → Courier New`

## Installation

### Option 1: Install System-Wide (Recommended)

1. Download Geist fonts from the official GitHub repository:
   - Visit: https://github.com/vercel/geist-font/releases/latest
   - Download the latest release ZIP file
   - Extract `Geist-Regular.otf`, `Geist-Medium.otf`, `Geist-SemiBold.otf`, `Geist-Bold.otf`
   - Extract `GeistMono-Regular.otf`, `GeistMono-Medium.otf`, `GeistMono-SemiBold.otf`, `GeistMono-Bold.otf`

2. Install the fonts on Windows:
   - Right-click each `.otf` file
   - Select "Install" or "Install for all users"
   - Restart the ExpenseTracker application

### Option 2: Use Fallback Fonts

If Geist fonts are not installed, the application will automatically fall back to:
- **Segoe UI** (Windows system font) for sans-serif text
- **Consolas** (Windows system font) for monospace text

## Font Weights Used

- **Regular (400)**: Body text, labels
- **Medium (500)**: Secondary headings, buttons
- **SemiBold (600)**: Headings, stats values, important text
- **Bold (700)**: Major headings (if needed)

## Resources

- **Official Website**: https://vercel.com/font
- **GitHub Repository**: https://github.com/vercel/geist-font
- **License**: SIL Open Font License 1.1 (free for personal and commercial use)

## Notes

- The application is designed to work with or without Geist fonts installed
- All font references use the `{StaticResource PrimaryFont}` and `{StaticResource MonoFont}` resource keys
- Font configuration is centralized in `Themes/ModernTheme.xaml`
