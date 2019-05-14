# VR-Tai-Chi
VR Tai Chi training game created in Unity
Video of gameplay: https://viewsync.net/watch?v=AVCKA-uNj8Y&t=0&v=W9MlAznD7bM&t=11

# Workflow Tips
## Branching

1. First switch to dev branch, and do a git fetch and pull to update your local repo to the latest.

```
git checkout dev
git fetch --all
git pull
```

2. Create a new branch for the specific feauture you are working on.

```
git checkout -b my-new-feature-branch
```

3. Track all the new files by adding them to the repo and commit your changes.

```
git add .
git commit -a -m "a commit message"
```

4. Push your branch to the repo and create a pull request from github

```
git push origin my-new-feature-brach
```

5. Make sure to switch back to the dev branch and create another branch before starting to work on aother feauture.

```
git checkout dev
```
