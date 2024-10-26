# Bede Lottery Game

## Assumptions
![image](https://github.com/user-attachments/assets/032f4e1c-d181-48b1-b3d4-f09060a1f61b)

The above sentence from the task description seems quite confusing to me. I wrote the system with the assumption that it meant:
- **If the total amount of prize money for a prize tier is not exactly divisible by the number of winners of that tier...**.

**Penny** as an `int` is used as the unit of currency for easy calculation and representation of money in the system. 

## Design

<img align="left" width="350" src="https://github.com/user-attachments/assets/e06e760a-690b-4637-8ab3-a2bf8596c821" />

- There are four solution folders: `Application`, `DAL`, `Domain`, and `Presentation` with relevant projects created under each of them. The idea is to follow the DDD pattern.

- `GameEngine` class serves as the orchestrator of the game while `GameStrategy` class is in charge of calculation and deciding which formula/method to use.

- `PlayerRepository` and `TicketRepository` are created to store data in memory. Both repositories are stateful `Singleton` instances. A `SemaphoreSlim` is used to ensure changing state is thread-safe.

- A `Unit of Work` class was not included as it seems a bit overkill for only persisting data in memory. 

- The purpose of the `Bede.SimplifiedLottery.IoC` project is to provide a central Inversion of Control point for dependency injection.

- `appsettings.json` is for the settings and configurations of the game, and is mapped to the `GameSettings` class. `GameSettings` is a bit of a chunky class because I wanted to do some in-place validation of the configuration.

