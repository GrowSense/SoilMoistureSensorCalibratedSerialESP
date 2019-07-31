#!/bin/bash

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "dev" ];  then
  echo "Graduating dev branch to master branch"

  echo "  Fetching from origin..."
  git fetch origin || exit 1

  echo "  Checking out master branch..."
  git checkout master || exit 1

  echo "  Merging dev branch into master branch..."
  git merge -X theirs origin/dev || exit 1

  echo "  Pushing updated master branch back to origin..."
  git push origin master || exit 1

  echo "  Checking out dev branch..."
  git checkout dev || exit 1

  echo "The 'dev' branch has been graduated to the 'master' branch"
else
  echo "You must be in the 'dev' branch to graduate to the 'master' branch, but currently in the '$BRANCH' branch. Skipping."
fi
