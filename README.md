# Task Manager (C# Console App)

A simple console-based task manager written in C# with JSON file persistence.

## Features

- Create tasks
- Edit tasks
- Delete tasks
- Mark tasks as completed
- Filter tasks by:
  - priority
  - completion status
- Persistent storage using `tasks.json`
- Input validation and error handling

## Technologies Used

- C#
- .NET
- JSON serialization (`System.Text.Json`)
- File I/O (`File.ReadAllText`, `File.WriteAllText`)

## Project Structure

- `TaskItem` — task model (data structure)
- `Logic` — user input handling and validation
- `TaskManager` — core task operations (CRUD, save/load)
- `Program` — main application loop

## How to Run

1. Clone the repository:
```bash
git clone <repository-url>
