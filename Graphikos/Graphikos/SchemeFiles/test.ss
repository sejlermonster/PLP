(load "Scheme.ss")

;Line testfunction
(define test-line
  (lambda (candidate)
    (and (equal? (candidate 0 0 8 8) '(0 0 1 1 2 2 3 3 4 4 5 5 6 6 7 7 8 8))
         (equal? (candidate 8 8 0 0) '(0 0 1 1 2 2 3 3 4 4 5 5 6 6 7 7 8 8))
         (equal? (candidate 0 8 8 0) '(0 8 1 7 2 6 3 5 4 4 5 3 6 2 7 1 8 0))
         (equal? (candidate 8 0 0 8) '(0 8 1 7 2 6 3 5 4 4 5 3 6 2 7 1 8 0))
         (equal? (candidate 1 1 8 1) '(1 1 2 1 3 1 4 1 5 1 6 1 7 1 8 1))
         (equal? (candidate 8 1 1 1) '(1 1 2 1 3 1 4 1 5 1 6 1 7 1 8 1))
         (equal? (candidate 1 8 1 1) '(1 1 1 2 1 3 1 4 1 5 1 6 1 7 1 8))
         (equal? (candidate 1 1 1 8) '(1 8 1 7 1 6 1 5 1 4 1 3 1 2 1 1))
         )))

(define TestRunLine (test-line line))
(for-each display '(Linetest))
TestRunLine

;Rectangle testfuntion
(define test-rectangle
  (lambda (candidate)
    (and (equal? (candidate 0 0 3 3) '(0 3 3 3 0 2 3 2 0 1 3 1 0 0 3 0 0 0 0 3 1 0 1 3 2 0 2 3 3 0 3 3))
         (equal? (candidate 0 3 3 0) '(0 0 3 0 0 1 3 1 0 2 3 2 0 3 3 3 0 3 0 0 1 3 1 0 2 3 2 0 3 3 3 0))
         (equal? (candidate 3 0 0 3) '(3 3 0 3 3 2 0 2 3 1 0 1 3 0 0 0 0 0 0 3 1 0 1 3 2 0 2 3 3 0 3 3))
         (equal? (candidate 3 3 0 0) '(3 0 0 0 3 1 0 1 3 2 0 2 3 3 0 3 0 3 0 0 1 3 1 0 2 3 2 0 3 3 3 0))
         )))

(define TestRunRectangle (test-rectangle rectangle))
(for-each display '(Rectangletest))
TestRunRectangle

;Circle testfunction
(define test-circle
  (lambda (candidate)
    (and (equal? (candidate 5 5 4) '(5 9 5 9 5 1 5 1 9 5 1 5 9 5 1 5 6 9 4 9 6 1 4 1 9 6 1 6 9 4 1 4 7 9 3 9 7 1 3 1 9 7 1 7 9 3 1 3 8 8 2 8 8 2 2 2 8 8 2 8 8 2 2 2))
         (equal? (candidate 15 10 9) '(15 19 15 19 15 1 15 1 24 10 6 10 24 10 6 10 16 19 14 19 16 1 14 1 24 11 6 11 24 9 6 9 17 19 13 19 17 1 13 1 24 12
                                         6 12 24 8 6 8 18 19 12 19 18 1 12 1 24 13 6 13 24 7 6 7 19 18 11 18 19 2 11 2 23 14 7 14 23 6 7 6 20 18 10 18 20 2 10 2 23
                                         15 7 15 23 5 7 5 21 18 9 18 21 2 9 2 23 16 7 16 23 4 7 4 22 17 8 17 22 3 8 3 22 17 8 17 22 3 8 3))
         )))

(define TestRunCircle (test-circle circle))
(for-each display '(Circletest))
TestRunCircle

;Fill testfunction
(define test-fill
  (lambda (candidate)
    (and (equal? (candidate "Red" (rectangle 1 1 4 4)) '(4 4 4 3 4 2 4 1 3 4 3 3 3 2 3 1 2 4 2 3 2 2 2 1 1 4 1 3 1 2 1 1 1 1 2 1 3 1 4 1 1 2 2 2 3 2 4 2 1 3 2 3 3 3 4 3 1 4 2 4 3 4 4 4 "Red"))
         (equal? (candidate "Red" (rectangle 4 1 1 4)) '(4 4 4 3 4 2 4 1 3 4 3 3 3 2 3 1 2 4 2 3 2 2 2 1 1 4 1 3 1 2 1 1 1 1 2 1 3 1 4 1 1 2 2 2 3 2 4 2 1 3 2 3 3 3 4 3 1 4 2 4 3 4 4 4 "Red"))
         (equal? (candidate "Red" (rectangle 1 4 4 1)) '(4 1 4 2 4 3 4 4 3 1 3 2 3 3 3 4 2 1 2 2 2 3 2 4 1 1 1 2 1 3 1 4 1 4 2 4 3 4 4 4 1 3 2 3 3 3 4 3 1 2 2 2 3 2 4 2 1 1 2 1 3 1 4 1 "Red"))
         (equal? (candidate "Red" (rectangle 4 4 1 1)) '(4 1 4 2 4 3 4 4 3 1 3 2 3 3 3 4 2 1 2 2 2 3 2 4 1 1 1 2 1 3 1 4 1 4 2 4 3 4 4 4 1 3 2 3 3 3 4 3 1 2 2 2 3 2 4 2 1 1 2 1 3 1 4 1 "Red"))
         (equal? (candidate "Red" (circle 5 5 2)) '(3 4 4 4 5 4 6 4 7 4 3 6 4 6 5 6 6 6 7 6 4 3 5 3 6 3 4 7 5 7 6 7 3 5 4 5 5 5 6 5 7 5 3 5 4 5 5 5 6 5 7 5 5 3 5 7 "Red"))
         )))

(define TestRunFill (test-fill fill))
(for-each display '(Filltest))
TestRunFill

;Testfunction
(define test-boundingbox
  (lambda (candidate)
    (and (equal? (candidate 1 1 8 8) '(1 8 8 8 1 6 8 6 1 4 8 4 1 2 8 2 1 1 1 8 3 1 3 8 5 1 5 8 7 1 7 8))
         (equal? (candidate 1 8 8 1) '(1 1 8 1 1 3 8 3 1 5 8 5 1 7 8 7 1 8 1 1 3 8 3 1 5 8 5 1 7 8 7 1))
         (equal? (candidate 8 1 1 8) '(8 8 1 8 8 6 1 6 8 4 1 4 8 2 1 2 1 1 1 8 3 1 3 8 5 1 5 8 7 1 7 8))
         (equal? (candidate 8 8 1 1) '(8 1 1 1 8 3 1 3 8 5 1 5 8 7 1 7 1 8 1 1 3 8 3 1 5 8 5 1 7 8 7 1))
         )))

(define TestRunBB (test-boundingbox bounding-box))
(for-each display '(Boundingboxtest))
TestRunBB

;testfunction
(define test-text
  (lambda (candidate)
    (and (equal? (candidate 7 3 "Så er der gang i den") '(7 3 "Så er der gang i den")))))

(define TestRunText (test-text text-at))
(for-each display '(Texttest))
TestRunText

;testfunction
(define test-draw
  (lambda (candidate)
   (and (equal? (candidate "Red" (line 1 1 4 4) (rectangle 1 5 5 1)) '("Red" 1 1 2 2 3 3 4 4 1 1 5 1 1 2 5 2 1 3 5 3 1 4 5 4 1 5 5 5 1 5 1 1 2 5 2 1 3 5 3 1 4 5 4 1 5 5 5 1)))))

(define TestRunDraw (test-draw draw))
(for-each display '(Drawtest))
TestRunDraw