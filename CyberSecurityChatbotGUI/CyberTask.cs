using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityChatbotGUI
{
    public class CyberTask
    {
        // Unique ID — set by the database automatically
        public int TaskId { get; set; }

        // Short title for the task e.g. "Enable 2FA"
        public string Title { get; set; } = string.Empty;

        // Longer description of what the task involves
        public string Description { get; set; } = string.Empty;

        // Optional reminder date e.g. "in 3 days" or "2026-07-01"
        public string ReminderDate { get; set; } = string.Empty;

        // Whether the task has been marked as done
        public bool IsCompleted { get; set; } = false;

        // When the task was created — set automatically
        public string CreatedAt { get; set; } = string.Empty;

        public string ToDisplayString()
        {
            string status = IsCompleted ? "✅ Done" : "⏳ Pending";
            string reminder = string.IsNullOrWhiteSpace(ReminderDate) ? "None" : ReminderDate;

            return status + " | " + Title + "\n" +
                   "     📝 " + Description + "\n" +
                   "     ⏰ Reminder: " + reminder + "\n" +
                   "     🗓️ Created: " + CreatedAt;
        }
    }
}
