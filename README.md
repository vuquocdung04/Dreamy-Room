# 🏡This is a *clone* of Dreamy Room! by ABI Studio.

## 👀Preview: 📄[Docs](https://docs.google.com/spreadsheets/d/1QdVYWCZSUDpdcWH1JoGkIA7Svf1iWrXe4Rl06xYT-0g/edit?usp=sharing) | ▶️[Demo](https://markdownlivepreview.com/)

## ✨Features:
1. 🧩**Game Logic**:
    * Multi-Snap & Conditions: Items can snap to multiple positions with independent validation rules.
    * Mask-based cleaning (e.g., scrub paint with a broom via Sprite Mask).
    * Multi-phase levels.
    * Blocks world interaction during popups, win/lose, etc.
2. 🚀**Boosters**: 
    * Hint, Magnifier, Frozen, Box Buffer, Time Buffer, Double Star.
3. 🎨**UI & Feedback**:
    * 35+ Popups.
    * Effects: Scene/Popup transitions, celebrations, placement feedback, star rewards, etc.
    * Spine Animations: Booster, Character reactions, etc.
4. ⏳**Time Manager**:
    * Auto-reset daily quests & rewards.
    * Heart system: +1 every 30 min until full. Unlimited mode skips consumption.
5. 🌍**Localization**:
    * Configurable via Google Sheets.
    * Supports remote (online) and local (CSV) loading.
6. 💾**Data Management**:
    * Game Data: Scriptable Object.
    * User Data: Saved as JSON and PlayerPrefs.
7. 📈**Performance Optimizations**:
    * Sprite Atlas: < 30 draw calls.
    * Addressable (load/unload level prefabs)