# Cybersecurity Awareness Chatbot — Part 3 / Full POE
A C# WPF desktop application that completes the Cybersecurity Awareness Chatbot by adding a Task Assistant, Cybersecurity Quiz, NLP Simulation, and Activity Log for South African citizens.

---

## GUI Application
The app launches a styled WPF window with a dark cybersecurity theme containing four tabs.

**On startup the app will:**
1. Play a WAV voice greeting automatically
2. Show a name entry dialog
3. Load the main chat window with your name personalised throughout
4. Connect to SQL Server and load your saved tasks

> **Note:** This is a Windows-only WPF application. `System.Media.SoundPlayer` requires Windows.

> **Note:** SQL Server (SQLEXPRESS) must be running on your machine for the Tasks tab to work.

---

## Database Setup
1. Open **SQL Server Management Studio (SSMS)**
2. Connect to `.\SQLEXPRESS` using Windows Authentication
3. Click **New Query** and run the following:

```sql
CREATE DATABASE CyberBotDB;
GO
USE CyberBotDB;
GO
CREATE TABLE Tasks (
    TaskId       INT IDENTITY(1,1) PRIMARY KEY,
    Title        NVARCHAR(200) NOT NULL,
    Description  NVARCHAR(500) NOT NULL,
    ReminderDate NVARCHAR(100) NULL,
    IsCompleted  BIT NOT NULL DEFAULT 0,
    CreatedAt    DATETIME NOT NULL DEFAULT GETDATE()
);
GO
```

4. Press **F5** in SSMS to run the script

---

## Features Implemented

| Task | Feature | File |
|------|---------|------|
| Task 1 | Task Assistant — add, view, complete, delete tasks with SQL Server | `DatabaseHelper.cs`, `CyberTask.cs`, `MainWindow.xaml.cs` |
| Task 2 | Cybersecurity Quiz — 12 questions, scoring, feedback, results | `QuizBank.cs`, `QuizQuestion.cs`, `MainWindow.xaml.cs` |
| Task 3 | NLP Simulation — detects varied phrasing and extracts intent | `NlpProcessor.cs`, `ChatbotEngine.cs` |
| Task 4 | Activity Log — timestamps, last 10 actions, viewable in tab and chat | `ActivityLog.cs`, `MainWindow.xaml.cs` |

---

## Tabs Overview

### 🤖 Chat Tab
The full chatbot from Parts 1 and 2 with NLP now layered on top.

Type any of the following keywords or click the quick-topic chips:
- `password` — strong password tips
- `phishing` — how to spot phishing attacks
- `safe browsing` — browsing the internet safely
- `malware` — protecting against malicious software
- `social engineering` — recognising manipulation tactics
- `2fa` — setting up two-factor authentication
- `privacy` — protecting your personal information
- `scam` — identifying and avoiding online scams
- `help` — shows all available topics

**You can also ask:**
- `How are you?`
- `What is your purpose?`
- `What can I ask you?`

**After any response try:**
- `tell me more` — get another tip on the same topic
- `give me another tip` — same as above

---

### 📋 Tasks Tab
A cybersecurity task assistant backed by SQL Server.

- Add tasks with a title, description, and optional reminder
- View all tasks with pending or completed status
- Mark tasks as complete — card border turns green
- Delete tasks from the list and database
- All data persists between sessions

**Example interaction:**
```
User:    Add task - Review privacy settings
CyberBot: Task added! Would you like a reminder?
User:    Yes, remind me in 3 days.
CyberBot: Got it! I'll remind you in 3 days.
```

---

### 🎮 Quiz Tab
A 12-question cybersecurity knowledge quiz.

- Mix of multiple choice and true/false questions
- Topics cover phishing, passwords, safe browsing, malware, 2FA, social engineering, and ransomware
- One question shown at a time
- Immediate feedback and explanation after each answer
- Correct answers highlighted green, wrong answers highlighted red
- Score tracked throughout with a final results screen

**Score feedback:**
| Percentage | Message |
|-----------|---------|
| 100% | 🏆 Perfect score! You're a cybersecurity expert! |
| 80–99% | 🌟 Great job! You have strong cybersecurity knowledge! |
| 60–79% | 👍 Good effort! Keep learning to stay safe online. |
| 40–59% | 📚 Keep studying! Cybersecurity knowledge is important. |
| 0–39% | 💪 Don't give up! Review the topics and try again. |

---

### 📜 Log Tab
A session activity log that records all significant actions.

- Tracks: chat messages, tasks added/completed/deleted, quiz started/completed
- Timestamps on every entry
- Displays the last 10 actions
- Refresh and clear buttons
- Also accessible by typing `show activity log` in the Chat tab

---

## NLP Simulation
The chatbot understands varied ways of phrasing requests:

| What you type | What the bot detects |
|---|---|
| `add a task to enable 2fa` | AddTask — extracts title automatically |
| `remind me to update my password` | SetReminder — extracts timeframe |
| `show my tasks` | ViewTasks — directs to Tasks tab |
| `quiz me` | StartQuiz — directs to Quiz tab |
| `what have you done for me` | ShowActivityLog — shows log in chat |
| `hello` | Greeting — personalised response |
| `thanks` | ThankBot — positive response |

---

## Sentiment Detection
The chatbot detects your mood and responds empathetically:

| Mood | Example Input | Bot Reaction |
|------|--------------|--------------|
| Worried | "I'm worried about scams" | Reassuring prefix + tip |
| Curious | "I'm curious about phishing" | Enthusiastic prefix + tip |
| Frustrated | "I'm frustrated with passwords" | Empathetic prefix + tip |
| Confused | "I don't understand 2fa" | Clear explanation prefix + tip |
| Happy | "Thanks, that's great!" | Positive prefix + tip |

---

## Memory and Recall
The chatbot remembers information you share during the conversation:

**Example:**
```
User:    "I'm interested in privacy"
CyberBot: "Great! I'll remember that you're interested in privacy..."

Later:
User:    "privacy"
CyberBot: "Since you're interested in privacy, this tip is especially relevant for you!..."
```
The status bar at the bottom of the window also updates to show your remembered topic.

---

## Project Structure
```
CyberSecurityChatbotGUI/
├── App.xaml                  # WPF application entry point
├── App.xaml.cs               # App code-behind
├── MainWindow.xaml           # Main window with all 4 tabs
├── MainWindow.xaml.cs        # All tab event handling and bubble rendering
├── NameDialog.xaml           # Startup name entry dialog UI
├── NameDialog.xaml.cs        # Name dialog logic
│
├── ── Part 2 Files ────────────────────────────────────
├── ChatbotEngine.cs          # Core response logic — orchestrates all features
├── ResponseBank.cs           # All responses, delegate, random picker
├── MemoryStore.cs            # User memory and recall
├── SentimentDetector.cs      # Mood detection and empathetic prefixes
├── ErrorHandler.cs           # Input validation, typo detection, fallbacks
│
├── ── Part 3 Files ────────────────────────────────────
├── CyberTask.cs              # Task model class
├── DatabaseHelper.cs         # SQL Server CRUD operations
├── QuizQuestion.cs           # Quiz question model class
├── QuizBank.cs               # All 12 quiz questions
├── NlpProcessor.cs           # NLP intent detection and extraction
├── ActivityLog.cs            # Session activity logging
│
├── CyberSecurityChatbotGUI.csproj
└── README.md
```

---

## YouTube Link:

https://youtu.be/IzV3kDJAQlY
