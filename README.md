# ARStudios

## Getting Started

### Prerequisites
- **GitHub Desktop**: Ensure you have [GitHub Desktop](https://desktop.github.com/) installed. We’ll be using it to clone the repository, create branches, and manage pull requests.
- **Unity**: Make sure the correct version of Unity is installed, as specified in [Lab 1](https://qmplus.qmul.ac.uk/mod/resource/view.php?id=2088344).

### Repository Setup

1. **Clone the Repository**  
   Open GitHub Desktop and clone this repository to your local machine.  
   - Go to `File > Clone repository` and select this repo from GitHub or paste the URL.

2. **Create a Branch from an Issue**  
   Every task or feature you work on must have a corresponding **Issue** created in the [Issues section](https://github.com/users/rayanbahadur/projects/3). To begin working on a task:
   - **Create a branch directly from the Issue** you’re working on. This will automatically link the branch to the issue.
   - To do this, go to the Issue page, click the button that allows you to create a branch from the issue, and choose a suitable branch name.

3. **Manual Branch Creation (If Needed)**  
   If for any reason you don’t create a branch from an issue:
   - In GitHub Desktop, select `Current Branch > New Branch` and name your branch in the format:  
     - `feature/your-feature-name` or `bugfix/your-fix-description`.

## Workflow

To keep our collaboration organized, follow this structured workflow:

### 1. **Create Issues and Tasks**
   - Any new feature, bug, or task must have a corresponding **Issue** created in the GitHub Issues tab.
   - Move the Issue to the relevant section of the **Projects Board** to track its progress (To Do, In Progress, etc.).

### 2. **Branching from Issues**
   - Always create a branch directly from the corresponding issue in GitHub, which will automatically link it to the task.
   - This helps maintain a clear connection between the work you're doing and the project management tasks.

### 3. **Work Locally**
   - Make your changes in Unity on your local machine.
   - Test everything thoroughly before committing.

### 4. **Commit and Push**
   - Commit your changes with clear and descriptive commit messages.
   - Push your branch to the remote repository using GitHub Desktop.

### 5. **Pull Request (PR)**
   - Once the task or feature is complete, open a **Pull Request** (PR) to merge your branch into `main`.
   - Request a review from another team member (if needed), otherwise review it yourself.

### 6. **Review and Merge**
   - Once reviewed and approved, merge the PR into `main`.
   - **Delete the branch** after merging to keep the repository clean.

### 7. **Update the Projects Board**
   - Move the related issue or task on the Projects board to `Done` after merging the PR and deleting the branch (usually done automatically).

## Additional Guidelines

- **Unity Project Settings**: Make sure all changes work within the current Unity version defined in the lab.
- **Branch Protection**: The `main` branch is protected. All changes must go through a Pull Request and review process before merging.

## Issues and Feature Requests

For any bugs or feature requests:
1. Create a new issue in the **Issues** tab.
2. Assign the issue to yourself or another team member.
3. Add the issue to the appropriate section of the **Projects Board**.
