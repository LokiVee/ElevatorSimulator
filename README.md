#Elevator Simulation  (C# Console Application)

A simulation program to model the movement and state management of elevators in a multi-floor building. The system handles multiple requests, allowing elevators to efficiently pick up and drop off passengers, adapting to real-time conditions.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [Testing](#testing)
- [Contact](#contact)

---

## Introduction

This project simulates a complex **elevator system** operating in a large building, handling passenger requests and optimizing transportation to reduce wait and travel times. It is built using **C#** and designed with **Solid principles** to make the system modular and maintainable, while ensuring optimal performance. The simulation manages multiple elevators, efficiently picking up and dropping off passengers based on real-time input.

---

## Features

- **Multi-elevator management**: Manages several elevators simultaneously in a large building.
- **Optimized passenger handling**: Reduces wait and travel times through efficient request prioritization.
- **Dynamic request handling**: Adjusts elevator routes in real time to account for new requests.
- **Real-time updates**: Provides immediate feedback to the user.
- **Interactive Elevator Support**: Provides immediate feedback to the user.

---

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- A C# IDE like [Visual Studio](https://visualstudio.microsoft.com/)
- Dot net 8

### Steps

1. Clone the repository:
   ```bash
   git clone [https://github.com/LokiVee/ElevatorSimulator.git]
   ```

2. Navigate to the project directory:
   ```bash
   cd ElevatorSimulator
   ```

3. Open the solution file (`.sln`) in your IDE (Visual Studio ).

4. Build the solution to restore dependencies and compile the project.

---

## Usage

### Running the Simulation

1. Run the application by right clicking on the solution and setting the start up project to ElevatorSimulator.Hosts.ConsoleApp.

2. You can input simulated requests, such as:
   - Requesting an elevator to a specific floor.
   - Number of passengers on each floor.
   - Current floor the user is request a floor from.


## Contributing

Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a new feature branch (`git checkout -b feature-branch`).
3. Commit your changes (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature-branch`).
5. Open a pull request.

---

## Testing

Run the unit tests to ensure the system functions as expected:

```bash
dotnet test
```

Test cases include:
- Verifying optimal pathing for elevators.
- Verifying The different states of the elevator
- Check the Elevator moves to the target floor
---


## Contact

For questions or issues, please contact:

- **Email**:vijay.manikkam766@gmail.com
- **GitHub**: [LokiVee] (https://github.com/LokiVee/ElevatorSimulator)


[![SimulationUnitTestBuilds](https://github.com/LokiVee/ElevatorSimulator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/LokiVee/ElevatorSimulator/actions/workflows/dotnet.yml)
