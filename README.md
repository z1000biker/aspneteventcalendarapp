# Event Calendar Application

A proof-of-concept ASP.NET Core 8.0 web application demonstrating API handling, calendar functionality, and comprehensive data validation.

## Features

### 🔌 RESTful API
- Full CRUD operations for event management
- Swagger/OpenAPI documentation
- Standardized JSON responses
- Comprehensive error handling
- FluentValidation integration

### 📆 Interactive Calendar
- FullCalendar.js integration
- Multiple views (month, week, day)
- Drag-and-drop event rescheduling
- Color-coded event categories
- Event creation, editing, and deletion

### ✅ Multi-Layer Validation
- Client-side validation with real-time feedback
- Server-side validation with ASP.NET Core
- FluentValidation for complex business rules
- Database constraints via Entity Framework Core

### 🎨 Modern UI
- Premium design with gradient backgrounds
- Smooth animations and transitions
- Responsive, mobile-first layout
- Glassmorphism effects

## Technology Stack

- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core** - ORM and database management
- **SQLite** - Lightweight database
- **FluentValidation** - Advanced validation
- **Swagger/OpenAPI** - API documentation
- **FullCalendar.js** - Interactive calendar
- **Bootstrap 5** - UI framework

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/aspneteventcalendarapp.git
   cd aspneteventcalendarapp
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to:
   - **Application**: http://localhost:5001
   - **API Documentation**: http://localhost:5001/swagger

## Project Structure

```
EventCalendarApp/
├── Controllers/
│   ├── HomeController.cs          # Main page controller
│   ├── EventsController.cs        # MVC controller for views
│   └── Api/
│       └── EventsApiController.cs # RESTful API endpoints
├── Models/
│   ├── Event.cs                   # Event entity model
│   ├── EventViewModel.cs          # View model for forms
│   └── ApiResponse.cs             # API response wrapper
├── Data/
│   ├── AppDbContext.cs            # EF Core database context
├── Validators/
│   └── EventValidator.cs          # FluentValidation rules
├── Views/
│   ├── Home/                      # Home page views
│   ├── Events/                    # Calendar and form views
│   └── Shared/                    # Layout and shared views
└── wwwroot/
    ├── css/                       # Custom styles
    └── js/                        # JavaScript files
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/eventsapi` | Get all events |
| GET | `/api/eventsapi/{id}` | Get event by ID |
| POST | `/api/eventsapi` | Create new event |
| PUT | `/api/eventsapi/{id}` | Update event |
| DELETE | `/api/eventsapi/{id}` | Delete event |

## Event Categories

Events are color-coded by category:
- **Work** - Blue
- **Personal** - Green
- **Meeting** - Orange
- **Holiday** - Red
- **General** - Purple

## Database

The application uses SQLite for data storage. The database is automatically created on first run with seed data including sample events.

## Validation Rules

- **Title**: Required, 3-200 characters
- **Description**: Optional, max 1000 characters
- **Start Date**: Required, must be before end date
- **End Date**: Required, must be after start date
- **Category**: Required, must be from predefined list
- **Duration**: Non-all-day events cannot exceed 24 hours

## License

This is a proof-of-concept application for demonstration purposes.

## Author

Created as a demonstration of ASP.NET Core capabilities including API development, calendar integration, and data validation.
