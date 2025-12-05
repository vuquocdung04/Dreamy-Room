# Dreamy Room - Clone Version
> Educational Project Original concept by ABI Game Studio

### 👀Preview:
[![Google Sheets](https://img.shields.io/badge/Google%20Sheets-34A853?style=for-the-badge&logo=google-sheets&logoColor=white)](https://docs.google.com/spreadsheets/d/1QdVYWCZSUDpdcWH1JoGkIA7Svf1iWrXe4Rl06xYT-0g/edit?usp=sharing)
 [![YouTube](https://img.shields.io/badge/YouTube-Demo-red?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/4ZYbHOiEdcs)
[![Unity 2022.3.45f1 LTS](https://img.shields.io/badge/Unity-2022.3.45f1%20LTS-blue?style=for-the-badge&logo=unity&logoColor=white)](https://unity.com/releases/editor/whats-new/2022.3.45)
## Features:
### 🧩**Game Logic**:
    1. Multi-Snap & Conditions: Items can snap to multiple positions with independent validation rules.
    2. Mask-based cleaning (e.g., scrub paint with a broom via Sprite Mask).
    3. Multi-phase levels.
    4. Blocks world interaction during popups, win/lose, etc.
### 🚀**Boosters**: 
     Hint, Magnifier, Frozen, Box Buffer, Time Buffer, Double Star.
### 🎨**UI & Feedback**:
    1. 35+ Popups.
    2. Effects: Scene/Popup transitions, celebrations, placement feedback, star rewards, etc.
    3. Spine Animations: Booster, Character reactions, etc.
### ⏳**Time Manager**:
    1. Auto-reset daily quests & rewards.
    2. Heart system: +1 every 30 min until full. Unlimited mode skips consumption.
### 🌍**Localization**:
    1. Configurable via Google Sheets.
    2. Supports remote (online) and local (CSV) loading.
### 💾**Data Management**:
    1. Game Data: Scriptable Object.
    2. User Data: Saved as JSON and PlayerPrefs.
### 📈**Performance Optimizations**:
    1. Sprite Atlas: < 30 draw calls.
    2. Addressable (load/unload level prefabs)