angular.module('QuizApp', []).controller('QuizCtrl', function ($scope, $http) {
        $scope.answered = false;
        $scope.title = "loading question...";
       // $scope.options = [];
        $scope.selectedAnswer = '';
        $scope.working = false;
        $scope.AnimalId = 0;
        $scope.FactId = 0;
        $scope.AnimalName = '';

        $scope.answer = function () {
            return $scope.answerVal ? 'yes' : 'no';
        };

        $scope.nextQuestion = function () {
            $scope.working = true;
            $scope.answered = false;
            $scope.title = "";
           // $scope.options = [];
            var config = {
                params: { animalId: $scope.AnimalId, factId: $scope.FactId, selectedAnswer: $scope.selectedAnswer }
            };


            $http.get("/api/question/get",config).then(function (data, status, headers, config) {
                // $scope.options = data.options;
                if (data.data != null) {
                    if (data.data.AnimalName != undefined && data.data.AnimalName != '') {
                        $scope.AnimalName = "The Animal is "+ data.data.AnimalName;
                        $scope.answered = true;

                    }
                    else {
                        $scope.title ="Does the animal " + data.data.Facts;
                        $scope.AnimalId = data.data.AnimalId;
                        $scope.FactId = data.data.FactId;
                        // $scope.answered = false;
                        //  $scope.working = false;
                    }
                }
                else {

                    $scope.AnimalName = "Animal not found";
                    $scope.answered = true;
                }
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... something went wrong";
                $scope.working = false;
            });
        };

        $scope.sendAnswer = function (option) {
            $scope.working = true;
            $scope.answered = true;

            $http.post('/api/question', { 'AnimalId': option.questionId, 'FactId': option.id }).success(function (data, status, headers, config) {
                $scope.correctAnswer = (data === true);
                $scope.working = false;
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... something went wrong";
                $scope.working = false;
            });
        };

    }


);